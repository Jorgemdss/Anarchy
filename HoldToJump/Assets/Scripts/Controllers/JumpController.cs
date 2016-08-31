using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Interfaces;
using System.Text;

public class JumpController : MonoBehaviour, IUpdatableUI
{
    #region vars
    /// <summary>
    /// The right rotation path.
    /// </summary>
    private const string rightRotationPath = "23412";

    /// <summary>
    /// The left rotation path.
    /// </summary>
    private const string leftRotationPath = "21432";

    private const string facingUpPathNumber = "1";
    private const string facingRightPathNumber = "2";
    private const string facingDownPathNumber = "3";
    private const string facingLeftPathNumber = "4";

    Rigidbody2D rigidBody2D;
        
    float playerRotation;

    bool isMouseDown;
    bool isGrounded;
    bool canJump;

    /// Rotation vars    
    bool hasFacedDown;
    bool hasFacedLeft;
    bool hasFacedUp;
    bool canUpdateRotationRight;
   
    StringBuilder rotationPath2;

    byte doubleJumpCounter;

    /// <summary>
    /// The ammount of time that the player is holding the jump.
    /// </summary>
    float timeMultiplier;

    /// <summary>
    /// Possible max holding time.
    /// </summary>
    float maxHoldTimeMultiplier;

    /// <summary>
    /// The force to multiply with the timeMultiplier.
    /// </summary>
    float timeMultiplierBaseForce;

    Text facingDirectionText;
    Text jetPackFuelText;

    public enum JumpType
    {
        NormalJump,
        DoubleJump,
        JetPack
    }
    public enum FacingDirection
    {
        None,
        FacingUp,
        FacingRight,
        FacingDown,
        FacingLeft
    }
    #endregion

    #region Properties

    public static int BaseJumpForce { get; set; }

    public static JumpType PlayerJumpType { get; set; }

    public byte MaxDoubleJumps { get; set; }
    public FacingDirection PlayerFacingDirection { get; set; }

    public int FullRotationCounter { get; set; }

    public int JetPackFuel { get; set; }

    #endregion

    void Start()
    {
        MaxDoubleJumps = 2;  // Max double jums is 2, maybe powerups will change this.
        isGrounded = false;
        BaseJumpForce = 175;
        rotationPath2 = new StringBuilder();
        maxHoldTimeMultiplier = 0.5f;
        timeMultiplierBaseForce = 400;
        PlayerJumpType = JumpType.NormalJump;
        rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        facingDirectionText = GameObject.Find("FacingValue").GetComponent<Text>();
        jetPackFuelText = GameObject.Find("FuelValue").GetComponent<Text>();
        JetPackFuel = 100; ;

        UpdatePlayerRotation();

    }

    void Update()
    {
        UpdatePlayerRotation();

        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
        }

        /// Start counting the hold jump force.
        if (isMouseDown)
        {
            timeMultiplier += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (PlayerJumpType != JumpType.JetPack)
            {

                isMouseDown = false;
                if (timeMultiplier > maxHoldTimeMultiplier)
                {
                    timeMultiplier = maxHoldTimeMultiplier;
                }
                Jump(BaseJumpForce, timeMultiplier, PlayerJumpType);

                timeMultiplier = 0;
            }
        }
        
        /// Jetpack interaction
        /// TODO : Fuel meter so the jetpack is not infitite.        
        if (Input.GetMouseButton(0) && PlayerJumpType == JumpType.JetPack)
        {
            UseJetPack();
        }
    }

    /// <summary>
    /// Apply jump action based on the type
    /// </summary>
    /// <param name="baseJumpForce">base jump force</param>
    /// <param name="timeMultiplier">the time that the player held the button down</param>
    /// <param name="jumpType">Type of jump action to perform</param>
    void Jump(int baseJumpForce, float timeMultiplier, JumpType jumpType)
    {
        var totalJumpForce = baseJumpForce + timeMultiplier * timeMultiplierBaseForce;
        var rigidBody2D = gameObject.GetComponent<Rigidbody2D>();

        if (isGrounded || canJump)
        {
            /// Normal Jump
            if (jumpType == JumpType.NormalJump)
            {
                rigidBody2D.AddForce(new Vector2(0, totalJumpForce));
                canJump = false;
            }
            /// Double JUMP
            else if (jumpType == JumpType.DoubleJump)
            {
                canJump = true;
                doubleJumpCounter++;

                if (doubleJumpCounter == MaxDoubleJumps)
                {
                    canJump = false;
                }

                rigidBody2D.AddForce(new Vector2(0, totalJumpForce));
            }
        }

        isGrounded = false;
    }

    void UseJetPack()
    {
        if (JetPackFuel > default(int))
        {
            JetPackFuel-=2;
            UpdateInterfaceText(jetPackFuelText, JetPackFuel.ToString());
            rigidBody2D.AddForce(new Vector2(0, 25));
        }
    }

    #region Private methods

    /// <summary>
    /// Updates player rotation, writes the path and detects if a full rotation was made.
    /// </summary>
    void UpdatePlayerRotation()
    {
        playerRotation = transform.localEulerAngles.z;
        var lastFacingDirecton = PlayerFacingDirection;

        /// Facing UP
        if (playerRotation < 45 || playerRotation > 315)
        {
            PlayerFacingDirection = FacingDirection.FacingUp;
            PlayerJumpType = JumpType.DoubleJump;
            if (lastFacingDirecton != PlayerFacingDirection && !hasFacedUp)
            {
                rotationPath2.Append(facingUpPathNumber);
                hasFacedUp = true;
                DetectFullRotation();
            }
        }

        /// Facing RIGHT
        if (playerRotation < 315 && playerRotation > 225)
        {
            PlayerFacingDirection = FacingDirection.FacingRight;
            PlayerJumpType = JumpType.NormalJump;

            if (canUpdateRotationRight)
            {
                rotationPath2.Append(facingRightPathNumber);
                canUpdateRotationRight = false;
                DetectFullRotation();
                hasFacedDown = false;
                hasFacedLeft = false;
                hasFacedUp = false;
            }
        }

        /// Facing DOWN
        if (playerRotation < 225 && playerRotation > 135)
        {
            PlayerFacingDirection = FacingDirection.FacingDown;
            PlayerJumpType = JumpType.JetPack;
            if (lastFacingDirecton != PlayerFacingDirection && !hasFacedDown)
            {
                rotationPath2.Append(facingDownPathNumber);
                hasFacedDown = true;
                DetectFullRotation();
            }
        }

        /// Facing LEFT
        if (playerRotation < 135 && playerRotation > 45)
        {
            PlayerFacingDirection = FacingDirection.FacingLeft;
            PlayerJumpType = JumpType.NormalJump;
            if (lastFacingDirecton != PlayerFacingDirection && !hasFacedLeft)
            {
                rotationPath2.Append(facingLeftPathNumber);
                hasFacedLeft = true;
                DetectFullRotation();
            }
        }

        UpdateInterfaceText(facingDirectionText, PlayerFacingDirection.ToString());
    }

    void DetectFullRotation()
    {
        if (hasFacedDown
            && hasFacedLeft
            && hasFacedUp
            && (rotationPath2.ToString().Contains(rightRotationPath)
                || rotationPath2.ToString().Contains(leftRotationPath)))
        {
            FullRotationCounter++;
            hasFacedDown = false;
            hasFacedLeft = false;
            hasFacedUp = false;

            rotationPath2.Remove(default(int),rotationPath2.Length);
            rotationPath2.Append(facingRightPathNumber);
        }
    }

    void DetectIfLandedOnEdge()
    {
        // AngularVelocity < 0 = rotating right
        var isRotatingClockwise = rigidBody2D.angularVelocity < 0 ? true : false;

        var torque = isRotatingClockwise ? -35f : 35f;

        // Flag that indicates if the square landed on an edge
        var landedOnEdge =
            (playerRotation > 215 && playerRotation < 235)
            || (playerRotation > 125 && playerRotation < 145)
            || (playerRotation > 35 && playerRotation < 60)
            || (playerRotation > 300 && playerRotation < 325);

        if (landedOnEdge)
        {
            rigidBody2D.AddTorque(torque);
        }
    }

    #endregion

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            isGrounded = true;
            doubleJumpCounter = 0;
            JetPackFuel = 100;
            UpdateInterfaceText(jetPackFuelText, JetPackFuel.ToString());
            DetectIfLandedOnEdge();
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            canUpdateRotationRight = true;
        }
    }

    public void UpdateInterfaceText(Text t, string c)
    {
        GameInterfaceController.UpdatePlayerRotationUI(t, c);
    }
}
