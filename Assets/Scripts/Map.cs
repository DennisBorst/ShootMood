using UnityEditor;
using UnityEngine;

public class Map : MonoBehaviour {

    public Transform Middle => middle;

    [SerializeField] private Transform middle;
    [SerializeField] private Vector3 size;

    [SerializeField] private Transform testTransform;

    private float mapTop => middle.position.z + (size.z / 2);
    private float mapBottom => middle.position.z - (size.z / 2);
    private float mapLeft => middle.position.x - (size.x / 2);
    private float mapRight => middle.position.x + (size.x / 2);

    private Vector3 topLeftCorner => middle.position + (new Vector3(-size.x, 0, size.z) / 2);
    private Vector3 topRightCorner => middle.position + (new Vector3(size.x, 0, size.z) / 2);
    private Vector3 bottomLeftCorner => middle.position + (new Vector3(-size.x, 0, -size.z) / 2);
    private Vector3 bottomRightCorner => middle.position + (new Vector3(size.x, 0, -size.z) / 2);

    public (float minAngle, float maxAngle) GetAnglesTowardsMap(Vector3 position) {
        bool isInTopSection = position.z > mapTop;
        bool isInBottomSection = position.z < mapBottom;
        bool isInRightSection = position.x > mapRight;
        bool isInLeftSection = position.x < mapLeft;

        (float minAngle, float maxAngle) angles = (isInTopSection, isInBottomSection, isInRightSection, isInLeftSection) switch {
            (true, _, true, _) => GetAngles(position, topLeftCorner, bottomRightCorner),    //TopRight 
            (_, true, _, true) => GetAngles(position, topLeftCorner, bottomRightCorner),    //BottomLeft

            (true, _, _, true) => GetAngles(position, topRightCorner, bottomLeftCorner),    //TopLeft
            (_, true, true, _) => GetAngles(position, topRightCorner, bottomLeftCorner),    //BottomRight


            (true, _, _, _) => GetAngles(position, topLeftCorner, topRightCorner),          //Top
            (_, true, _, _) => GetAngles(position, bottomLeftCorner, bottomRightCorner),    //Bottom
            (_, _, true, _) => GetAngles(position, topRightCorner, bottomRightCorner),      //Right
            (_, _, _, true) => GetAngles(position, topLeftCorner, bottomLeftCorner),        //Left

            _ => (0, 0)                                                                     //Unknown
        };

        return angles;
    }

    public Vector3 GetOppositePosition(Vector3 position) {
        Vector3 oppositePosition = middle.transform.position - position;
        oppositePosition.y = position.y;
        return oppositePosition;
    }

    public bool IsInMap(Vector3 position, float extraRadius = 0) {
        extraRadius /= 2;
        return !(position.x < mapLeft - extraRadius || position.x > mapRight + extraRadius || position.z > mapTop + extraRadius || position.z < mapBottom - extraRadius);
    }

    public Vector3 GetMapSize(float extraRadius = 0) {
        return new Vector3(size.x + extraRadius, 0, size.z + extraRadius);
    }

    private (float minAngle, float maxAngle) GetAngles(Vector3 position, Vector3 corner1, Vector3 corner2) {
        float angle1 = Quaternion.LookRotation(corner1 - position).eulerAngles.y;
        float angle2 = Quaternion.LookRotation(corner2 - position).eulerAngles.y;

        if (Mathf.Abs(angle1 - angle2) > 180) {
            float newAngle1 = Mathf.Max(angle1, angle2) - 360f;
            float newAngle2 = Mathf.Min(angle1, angle2);

            angle1 = newAngle1;
            angle2 = newAngle2;
        }

        return (Mathf.Min(angle1, angle2), Mathf.Max(angle1, angle2));
    }

    private void OnDrawGizmosSelected() {
        Handles.color = Color.yellow;
        Handles.DrawWireCube(middle.position, size);

        Handles.DrawWireCube(topLeftCorner, Vector3.one);
        Handles.DrawWireCube(topRightCorner, Vector3.one);
        Handles.DrawWireCube(bottomLeftCorner, Vector3.one);
        Handles.DrawWireCube(bottomRightCorner, Vector3.one);
    }

    private void OnDrawGizmos() {
        if (testTransform == null) { return; }
        Handles.color = Color.blue;
        Handles.DrawWireCube(testTransform.position, Vector3.one);

        (float minAngle, float maxAngle) angles = GetAnglesTowardsMap(testTransform.position);

        testTransform.localEulerAngles = new Vector3(0, angles.minAngle, 0);
        Handles.DrawLine(testTransform.position, testTransform.position + (testTransform.forward * 500f));
        Handles.Label(testTransform.position + (testTransform.forward * 4f), angles.minAngle.ToString());

        testTransform.localEulerAngles = new Vector3(0, angles.maxAngle, 0);
        Handles.DrawLine(testTransform.position, testTransform.position + (testTransform.forward * 500f));
        Handles.Label(testTransform.position + (testTransform.forward * 4f), angles.maxAngle.ToString());

        testTransform.localEulerAngles = Vector3.zero;
    }

    #region Singleton
    private static Map instance;
    private void Awake() {
        instance = this;
    }
    public static Map Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<Map>();
            }
            return instance;
        }
    }

    #endregion

}