using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
public enum DIRECTION { LEFT, DOWN, UP, RIGHT }
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    const string DIRECTION_X_NAME = "moveX", DIRECTION_Y_NAME = "moveY";

    public float moveSpeed;

    private bool isMoving;
    private Vector2 input;
    private Animator animator;
    private Rigidbody2D playerRigidbody;



    private bool isAttaking = false;
    private bool isAttackBlocked = false;
    private float attackDelay = 0.4f;
    protected bool isImortal = false;
    protected float imortalTime = 0.5f;

    private Collision2D curCollider;
    private SoundManager soundManager;

    private Weapon weapon;
    private int BaseDEF, BaseATK, BaseHP;
    private int BaseCritChance;
    private int curEXP;
    private int coin;

    public int curHP;
    public int level, nextLevelEXP;

    public int weaponInventoryIndex;
    public List<InventoryItem> inventory;

    public void RemoveInstancePlayer()
    {
        SetStartGamePlayerInfo();
        MainCameraController.instance.SetCameraStaticPos(Vector2.zero);
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            animator = GetComponent<Animator>();
            playerRigidbody = GetComponent<Rigidbody2D>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if (inventory == null)
        {
            SetStartGamePlayerInfo();
        }
        soundManager = SoundManager.instance;

    }
    public void SetBeginInventory()
    {
        weapon = (Weapon)ItemManager.Instance.GetItem("Base Iron Sword");

        inventory = new List<InventoryItem>
        {
            new(weapon),
            new(ItemManager.Instance.GetItem("Normal HP Potion"),2)
        };

        weaponInventoryIndex = 0;
    }
    public void UseInventoryItem(int index)
    {
        if (inventory[index].item.itemType == ItemType.weapon)
        {
            weaponInventoryIndex = index;
            inventory[index].item.Use();
            return;
        }
        inventory[index].item.Use();
        inventory[index].amount--;
        if (inventory[index].amount == 0)
        {
            if (weaponInventoryIndex > index) weaponInventoryIndex--;
            inventory.RemoveAt(index);
        }
    }
    public void setActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
    public void HandleFixedUpdate()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = input.x != 0 ? 0 : Input.GetAxisRaw("Vertical");
        if (input != Vector2.zero && !isAttaking)
        {
            animator.SetFloat(DIRECTION_X_NAME, input.x);
            animator.SetFloat(DIRECTION_Y_NAME, input.y);

            Vector3 targetPos = transform.position + (new Vector3(input.x, input.y, 0)) * moveSpeed * Time.deltaTime;
            if (!GameController.instance.gameStoryManager.CheckPlayerCanMove(targetPos))
            {
                GameController.instance.SetGameState(GAME_STATE.DIALOG);
                DialogManager.instance.ShowDialog("May be not that way!");
                animator.SetBool("isMoving", false);
            }
            else
            {
                isMoving = true;
                animator.SetBool("isMoving", true);
                playerRigidbody.MovePosition(targetPos);
            }
        }


    }
    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
        if (input == Vector2.zero)
        {
            if (Input.GetKeyDown(KeyCode.Return) && curCollider != null)
            {
                curCollider.gameObject.GetComponent<Interactable>()?.Interact();
                animator.SetBool("isMoving", false);
            }
            if (isMoving)
            {
                isMoving = false;
                animator.SetBool("isMoving", false);
            }

        }


    }

    public void SetUpAttr()
    {
        BaseDEF = 1; BaseATK = 1; BaseHP = 7;
        curHP = BaseHP;
    }
    private void Attack()
    {
        if (isAttackBlocked) return;
        animator.SetTrigger("Attack");
        isAttaking = true;
        isAttackBlocked = true;
        StartCoroutine(DelayAttack());
        SoundManager.instance.PLayerAttack();
    }
    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        isAttackBlocked = false;

    }
    public void OnAttackAnimationEnd()
    {
        isAttaking = false;
    }
    public bool GetIsCrit()
    {
        int a = UnityEngine.Random.Range(0, 100);
        int cr = BaseCritChance + weapon.GetCritChance();
        return a < cr;
    }
    public int GetRealAttackDamage(bool isCrit)
    {
        int damage = (BaseATK + weapon.getATK());
        return isCrit ?
       (int)Math.Round(damage * 1.75f) : damage;

    }

    public void Hit(int damage)
    {
        if (!isImortal)
        {

            curHP -= damage - BaseDEF > 0 ? (damage - BaseDEF) : 0;

            if (curHP <= 0)
            {
                GameController.instance.SetGameState(GAME_STATE.DIED);
                return;
            }
            isImortal = true;
            StartCoroutine(DelayHit());
        }

    }
    protected IEnumerator DelayHit()
    {
        yield return new WaitForSeconds(imortalTime);
        isImortal = false;
    }

    public void CollectEXP(int collectedEXP)
    {
        curEXP = collectedEXP + curEXP;
        if (curEXP >= nextLevelEXP)
        {
            curEXP = curEXP - nextLevelEXP;
            LevelUp();
        }
    }
    private void LevelUp()
    {
        level += 1;
        SetPlayerAttrByLV(level);
        curHP = GetMaxHP();
        ScrollingMessageTextManager.instance.AddText("Level Up!");

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            curCollider = collision;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            curCollider = null;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectable"))
        {
            Item itemComponent = other.gameObject.GetComponent<Collectable>().Collect();

            if (AddItemToInventory(itemComponent))
            {
                Destroy(other.gameObject);
                SoundManager.instance.PlaySFX(0);
                ScrollingMessageTextManager.instance.AddText(itemComponent.itemName + " is collected !");
            }
        }
    }

    public bool AddItemToInventory(Item itemComponent)
    {
        bool isNew = true;
        foreach (var item in inventory)
        {
            if (item.item.name == itemComponent.name)
            {
                if (itemComponent.isStackble && item.amount < item.item.maxStack)
                {
                    isNew = false;
                    item.amount++;
                    break;
                }
                else return false;

            }
        }
        if (isNew) inventory.Add(new InventoryItem(itemComponent));
        return true;
    }

    //GET/SET BASIC ATTR
    public void SetStartGamePlayerInfo()
    {
        SetPlayerAttrByLV(1);
        curHP = BaseHP;
        coin = 100;
        curEXP = 0;
        SetBeginInventory();
    }
    public void SetPlayerAttrByLV(int lv)
    {
        BaseHP = 6 + (lv - 1) * 2;
        BaseDEF = lv;
        BaseATK = 2 + lv - 1;
        BaseCritChance = 3 + lv * 2;
        level = lv;
        nextLevelEXP = (int)(5 * Math.Pow(1.75f, lv));

    }
    public int GetATK()
    {
        return BaseATK + weapon.getATK();
    }
    public int GetCrit()
    {
        return BaseCritChance + weapon.GetCritChance();
    }
    public int GetDEF()
    {
        return BaseDEF;
    }
    public void SetCurWeapon(Weapon wp)
    {
        weapon = wp;

    }
    public int GetCoin()
    {
        return coin;
    }
    public void ChangeCoin(int changeCoin)
    {
        coin += changeCoin;
    }
    public void SetDirection(DIRECTION direction)
    {
        if (direction == DIRECTION.LEFT)
        {
            animator.SetFloat(DIRECTION_X_NAME, -1);
            animator.SetFloat(DIRECTION_Y_NAME, 0);
        }
        else if (direction == DIRECTION.RIGHT)
        {
            animator.SetFloat(DIRECTION_X_NAME, 1);
            animator.SetFloat(DIRECTION_Y_NAME, 0);
        }
        else if (direction == DIRECTION.UP)
        {
            animator.SetFloat(DIRECTION_X_NAME, 0);
            animator.SetFloat(DIRECTION_Y_NAME, 1);
        }
        else
        {
            animator.SetFloat(DIRECTION_X_NAME, 0);
            animator.SetFloat(DIRECTION_Y_NAME, -1);
        }
    }
    public void movePlayer(Vector2 vct)
    {
        SetIsMoving(false);
        transform.position = vct;
    }
    public void SetIsMoving(bool isMoving)
    {
        this.isMoving = isMoving;
        animator.SetBool("isMoving", isMoving);
    }
    public bool HealHP(int val)
    {
        if (curHP == BaseHP) return false;
        curHP = curHP + 6 > BaseHP ? BaseHP : curHP + 6;
        return true;
    }
    public int GetMaxHP() => BaseHP;
    public int GetCurEXP() => this.curEXP;


    //Reapawn and SetNewInstanceStateOfPlayer
    public void Respawn(Vector2 pos, int lv, int curEXP, int coin, List<InventoryItem> newInventory, int weaponInventoryIndex)
    {
        SetPlayerAttrByLV(lv);
        this.curEXP = curEXP;
        this.coin = coin;
        this.weaponInventoryIndex = weaponInventoryIndex;
        inventory = new List<InventoryItem>();
        foreach (var item in newInventory)
        {
            inventory.Add(item);
        }
        this.weapon = (Weapon)inventory[weaponInventoryIndex].item;
        movePlayer(pos);
        curHP = BaseHP;
        SetIsMoving(false);
    }
    public void LoadPlayerATTR(Vector2 pos, int curHP, int lv, int curEXP, int coin, List<SavedInventoryItem> newInventory, int weaponInventoryIndex)
    {
        SetPlayerAttrByLV(lv);
        this.curHP = curHP;
        this.curEXP = curEXP;
        this.coin = coin;
        this.weaponInventoryIndex = weaponInventoryIndex;
        inventory = new List<InventoryItem>();
        foreach (var item in newInventory)
        {
            inventory.Add(new InventoryItem(ItemManager.Instance.GetItem(item.name), item.amount));
        }
        this.weapon = (Weapon)inventory[weaponInventoryIndex].item;
        movePlayer(pos);
    }

}