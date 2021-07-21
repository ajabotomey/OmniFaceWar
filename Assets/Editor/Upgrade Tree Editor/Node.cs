using System;
using UnityEngine;
using UnityEditor;
using System.Text;
public class Node
{
    public Rect rect;
    public string title;
    public bool isDragged;
    public bool isSelected;

    // Rect for the title of the node
    public Rect rectID;

    // Two Rect for the name of the upgrade
    public Rect rectNameLabel;
    public Rect rectName;

    // Two Rect for the unlock field (1 for the label and another for the checkbox
    public Rect rectUnlockLabel;
    public Rect rectUnlocked;

    // Two Rect for the cost field (1 for the label and another for the text field
    public Rect rectCostLabel;
    public Rect rectCost;

    // Two Rect for the description of the upgrade
    public Rect rectDescriptionLabel;
    public Rect rectDescription;

    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;

    public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;

    // GUI Style for the title
    public GUIStyle styleID;

    // GUI Style for the fields
    public GUIStyle styleField;

    public Action<Node> OnRemoveNode;

    // Skill linked with the node
    public Upgrade upgrade;

    // Bool for checking if the node is unlocked or not
    private bool unlocked = false;

    // StringBuilder to create the node's title
    private StringBuilder nodeTitle;

    public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode, int id, bool unlocked, int cost, int[] dependencies)
    {
        rect = new Rect(position.x, position.y, width, height);
        style = nodeStyle;

        inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
        outPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);

        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        OnRemoveNode = OnClickRemoveNode;

        // Create new Rect and GUIStyle for our title and custom fields
        SetUpNodeRect(position, width, height);

        this.unlocked = unlocked;

        // We create the skill with the current node info
        upgrade = new Upgrade();
        upgrade.upgradeID = id;
        upgrade.unlocked = unlocked;
        upgrade.cost = cost;
        upgrade.upgradeDependencies = dependencies;

        // Create string with ID info
        nodeTitle = new StringBuilder();
        nodeTitle.Append("ID: ");
        nodeTitle.Append(id);
    }

    public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode, int id, bool unlocked, int cost)
    {
        rect = new Rect(position.x, position.y, width, height);
        style = nodeStyle;

        inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
        outPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);

        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        OnRemoveNode = OnClickRemoveNode;

        // Create new Rect and GUIStyle for our title and custom fields
        SetUpNodeRect(position, width, height);

        this.unlocked = unlocked;

        // We create the skill with the current node info
        upgrade = new Upgrade();
        upgrade.upgradeID = id;
        upgrade.unlocked = unlocked;
        upgrade.cost = cost;
        upgrade.upgradeDependencies = null;

        // Create string with ID info
        nodeTitle = new StringBuilder();
        nodeTitle.Append("ID: ");
        nodeTitle.Append(id);
    }

    public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode, int id, string name, string description, bool unlocked, int cost, int[] dependencies)
    {
        rect = new Rect(position.x, position.y, width, height);
        style = nodeStyle;

        inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
        outPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);

        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        OnRemoveNode = OnClickRemoveNode;

        // Create new Rect and GUIStyle for our title and custom fields
        SetUpNodeRect(position, width, height);

        this.unlocked = unlocked;

        // We create the skill with the current node info
        upgrade = new Upgrade();
        upgrade.upgradeID = id;
        upgrade.upgradeName = name;
        upgrade.upgradeDescription = description;
        upgrade.unlocked = unlocked;
        upgrade.cost = cost;
        upgrade.upgradeDependencies = dependencies;

        // Create string with ID info
        nodeTitle = new StringBuilder();
        nodeTitle.Append("ID: ");
        nodeTitle.Append(id);

        
    }


    private void SetUpNodeRect(Vector2 position, float width, float height)
    {
        // Row Height Breakdown: 
        // ID = 1 row
        // Upgrade Name = 2 rows
        // Upgrade description = 3 rows
        // Cost = 1 row
        // Unlocked = 1 row

        float rowHeight = height / 10;

        rectID = new Rect(position.x, position.y + rowHeight, width, rowHeight);
        styleID = new GUIStyle();
        styleID.alignment = TextAnchor.UpperCenter;

        rectNameLabel = new Rect(position.x, position.y + 2 * rowHeight, width / 2, rowHeight);
        rectName = new Rect(position.x + width / 2, position.y + 2 * rowHeight, 130, rowHeight * 2);

        rectDescriptionLabel = new Rect(position.x, position.y + 4 * rowHeight, width / 2, rowHeight);
        rectDescription = new Rect(position.x + width / 2, position.y + 4 * rowHeight, 130, rowHeight * 3);

        rectCostLabel = new Rect(position.x, position.y + 7 * rowHeight, width / 2, rowHeight);
        rectCost = new Rect(position.x + width / 2, position.y + 7 * rowHeight, 60, rowHeight);

        rectUnlocked = new Rect(position.x + width / 2, position.y + 8 * rowHeight, width / 2, rowHeight);
        rectUnlockLabel = new Rect(position.x, position.y + 8 * rowHeight, width / 2, rowHeight);

        styleField = new GUIStyle();
        styleField.alignment = TextAnchor.UpperRight;
    }

    public void Drag(Vector2 delta)
    {
        rect.position += delta;
        rectID.position += delta;
        rectNameLabel.position += delta;
        rectName.position += delta;
        rectDescriptionLabel.position += delta;
        rectDescription.position += delta;
        rectUnlocked.position += delta;
        rectUnlockLabel.position += delta;
        rectCost.position += delta;
        rectCostLabel.position += delta;
    }

    public void MoveTo(Vector2 pos)
    {
        rect.position = pos;
        rectID.position = pos;
        rectNameLabel.position = pos;
        rectName.position = pos;
        rectDescriptionLabel.position = pos;
        rectDescription.position = pos;
        rectUnlocked.position = pos;
        rectUnlockLabel.position = pos;
        rectCost.position = pos;
        rectCostLabel.position = pos;
    }

    public void Draw()
    {
        inPoint.Draw();
        outPoint.Draw();
        GUI.Box(rect, title, style);

        // Print the title
        GUI.Label(rectID, nodeTitle.ToString(), styleID);

        // Print the name field
        GUI.Label(rectNameLabel, "Upgrade Name: ", styleField);
        upgrade.upgradeName = GUI.TextField(rectName, upgrade.upgradeName);

        // Print the description field
        GUI.Label(rectDescriptionLabel, "Upgrade Description: ", styleField);
        upgrade.upgradeDescription = GUI.TextArea(rectDescription, upgrade.upgradeDescription);

        // Print the unlocked field
        GUI.Label(rectUnlockLabel, "Unlocked: ", styleField);
        if (GUI.Toggle(rectUnlocked, unlocked, ""))
            unlocked = true;
        else
            unlocked = false;

        upgrade.unlocked = unlocked;

        // Print the cost field
        GUI.Label(rectCostLabel, "Cost: ", styleField);
        int upgradeCost = int.Parse(upgrade.cost.ToString());
        upgrade.cost = int.Parse(GUI.TextField(rectCost, upgrade.cost.ToString()));
    }

    public bool ProcessEvents(Event e)
    {
        switch (e.type) {
            case EventType.MouseDown:
                if (e.button == 0) {
                    if (rect.Contains(e.mousePosition)) {
                        isDragged = true;
                        GUI.changed = true;
                        isSelected = true;
                        style = selectedNodeStyle;
                    } else {
                        GUI.changed = true;
                        isSelected = false;
                        style = defaultNodeStyle;
                    }
                }

                if (e.button == 1 && isSelected && rect.Contains(e.mousePosition)) {
                    ProcessContextMenu();
                    e.Use();
                }
                break;
            case EventType.MouseUp:
                isDragged = false;
                break;
            case EventType.MouseDrag:
                if (e.button == 0 && isDragged) {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;
        }

        return false;
    }

    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();
    }

    private void OnClickRemoveNode()
    {
        OnRemoveNode?.Invoke(this);
    }
}
