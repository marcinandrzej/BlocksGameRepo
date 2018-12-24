using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    private const int CELL_COUNT = 10;
    private const int BASE_TARGET = 20;
    private const int MIN_TARGET = 5;

    private Color32[] colors; 
    public Sprite spr;
    private int target;
    private int level;
    private int inPlay;

    private GameObject gamePanel;
    private GameObject[,] tab;
    private GuiManagerScript guiScr;

    private GameObject menuPanel;
    private GameObject menuButton1;
    private GameObject menuButton2;
    private GameObject menuButton1Text;
    private GameObject menuButton2Text;
    private GameObject menuText;

    private GameObject guiPanel;
    private GameObject targetView;
    private GameObject levelView;
    private GameObject inPlayView;
    private GameObject exitButton;

    private action onClick;

    void Awake()
    {
        colors = new Color32[5];
        colors[0] = new Color32(255,0,0,255);
        colors[1] = new Color32(0, 255, 0, 255);
        colors[2] = new Color32(0, 255, 255, 255);
        colors[3] = new Color32(255, 255, 0, 255);
        colors[4] = new Color32(255, 0, 255, 255);
        target = BASE_TARGET;
        level = 1;
        inPlay = CELL_COUNT * CELL_COUNT;
    }

    // Use this for initialization
    void Start()
    {
        SetUpGame();
        SetUpGui();
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void SetUpGui()
    {
        guiPanel = guiScr.CreatePanel(gameObject, "GuiPanel", new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector3(1, 1, 1),
            new Vector3(0, 0, 0), new Vector2(150, 400), new Vector2(100, 0), spr, new Color32(0, 0, 0, 0));
        levelView = guiScr.CreateText(guiPanel, "Level", new Vector2(0.5f, 1), new Vector2(0.5f, 1),
                new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(150, 50), new Vector2(0, -25), ("Level: " + level.ToString()),
                new Color32(0, 0, 0, 255));
        targetView = guiScr.CreateText(guiPanel, "Target", new Vector2(0.5f, 1), new Vector2(0.5f, 1),
                new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(150, 50), new Vector2(0, -75), ("Target: " + target.ToString()),
                new Color32(0, 0, 0, 255));
        inPlayView = guiScr.CreateText(guiPanel, "Blocks in play", new Vector2(0.5f, 1), new Vector2(0.5f, 1),
                new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(150, 50), new Vector2(0, -125), ("Blocks in play: " + inPlay.ToString()),
                new Color32(0, 0, 0, 255));
        exitButton = guiScr.CreateButton(guiPanel, "Blocks in play", new Vector2(0.5f, 0), new Vector2(0.5f, 0),
                new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(150, 50), new Vector2(0, 25), spr,
                new Color32(255, 255, 255, 255));
        guiScr.CreateText(exitButton, "ButtonText", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(150, 50), new Vector2(0, 0), ("EXIT"),
                new Color32(0, 0, 1, 255));
        exitButton.GetComponent<Button>().onClick.AddListener(delegate { Application.Quit(); });
    }

    private void UpdateGui()
    {
        levelView.GetComponent<Text>().text = "Level: " + level.ToString();
        targetView.GetComponent<Text>().text = "Target: " + target.ToString();
        inPlayView.GetComponent<Text>().text = "Blocks in play: " + inPlay.ToString();
    }

    private void SetUpGame()
    {
        guiScr = new GuiManagerScript();
        gamePanel = guiScr.CreatePanel(gameObject, "GamePanel", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector3(1, 1, 1), new Vector3(0, 0, 0),
           new Vector2(400, 400), new Vector2(0, 0), spr, new Color32(0, 150, 120, 255));
        tab = guiScr.FillWithButtons(gamePanel, CELL_COUNT, CELL_COUNT, spr, colors);

        onClick = new action(Execute);

        guiScr.SetAction(tab, onClick);
    }

    private void ShowEndMenu(bool win)
    {
        string text = "YOU WIN";
        string text2 = "Next Level";
        if (!win)
        {
            text = "YOU LOSE";
            text2 = "Play Again";
        }

            menuPanel = guiScr.CreatePanel(gameObject, "MenuPanel", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(400, 200), new Vector2(0, 100), spr,
                new Color32(255, 255, 255, 255));
            menuButton1 = guiScr.CreateButton(menuPanel, "MenuButton", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(200, 100), new Vector2(-100, -50), spr,
                new Color32(255, 255, 255, 255));
            menuButton2 = guiScr.CreateButton(menuPanel, "MenuButton2", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(200, 100), new Vector2(100, -50), spr,
                new Color32(255, 255, 255, 255));
            menuText = guiScr.CreateText(menuPanel, "MenuText", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(200, 100), new Vector2(0, 50), text,
                new Color32(0, 0, 0, 255));
            menuButton1Text = guiScr.CreateText(menuButton1, "Text", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(200, 100), new Vector2(0, 0), "EXIT",
                new Color32(0, 0, 0, 255));
            menuButton2Text = guiScr.CreateText(menuButton2, "Text", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
               new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(200, 100), new Vector2(0, 0), text2,
               new Color32(0, 0, 0, 255));

        menuButton1.GetComponent<Button>().onClick.AddListener(delegate { Application.Quit(); });

        if (win)
        {
            menuButton2.GetComponent<Button>().onClick.AddListener(delegate
            {
                Destroy(gamePanel);
                SetUpGame();
                target = Mathf.Max(target - 1, MIN_TARGET);
                level++;
                inPlay = CELL_COUNT * CELL_COUNT;
                UpdateGui();
                Destroy(menuPanel);
            });
        }
        else
        {
            menuButton2.GetComponent<Button>().onClick.AddListener(delegate
            {
                Destroy(gamePanel);
                SetUpGame();
                target = BASE_TARGET;
                level = 1;
                inPlay = CELL_COUNT * CELL_COUNT;
                UpdateGui();
                Destroy(menuPanel);
            });
        }
    }

    public void BlockGrid(bool enable)
    {
        for (int i = 0; i < CELL_COUNT; i++)
        {
            for (int j = 0; j < CELL_COUNT; j++)
            {
                if (tab[i,j] != null)
                {
                    tab[i, j].GetComponent<Button>().enabled = enable;
                }
            }
        }
    }

    public void Execute(GameObject button)
    {
        int x = button.GetComponent<ButtonScript>().X;
        int y = button.GetComponent<ButtonScript>().Y;
        int colIndx = button.GetComponent<ButtonScript>().ColorIndex;
        StartCoroutine(UpdateCoroutine(x, y, colIndx));
    }

    private void CheckNeighbours(int x, int y, int colIndex, List<GameObject> toDestroy)
    {
        if (CheckBlock((x - 1), y, colIndex) && !toDestroy.Contains(tab[x - 1, y]))
        {
            toDestroy.Add(tab[(x - 1), y]);
            CheckNeighbours((x - 1),  y, colIndex, toDestroy);
        }
        if (CheckBlock((x + 1), y, colIndex) && !toDestroy.Contains(tab[x + 1, y]))
        {
            toDestroy.Add(tab[(x + 1), y]);
            CheckNeighbours((x + 1), y, colIndex, toDestroy);
        }
        if (CheckBlock(x, (y - 1), colIndex) && !toDestroy.Contains(tab[x, (y - 1)]))
        {
            toDestroy.Add(tab[x, (y - 1)]);
            CheckNeighbours(x, (y - 1), colIndex, toDestroy);
        }
        if (CheckBlock(x, (y + 1), colIndex) && !toDestroy.Contains(tab[x, (y + 1)]))
        {
            toDestroy.Add(tab[x, (y + 1)]);
            CheckNeighbours(x, (y + 1), colIndex, toDestroy);
        }
    }

    private bool CheckBlock(int x, int y, int colIndex)
    {
        if (x >= 0 && x < CELL_COUNT && y >= 0 && y < CELL_COUNT)
        {
            if (tab[x, y] != null)
            {
                if (tab[x, y].GetComponent<ButtonScript>().ColorIndex == colIndex)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void BlocksFall()
    {
        for (int x = 0; x < CELL_COUNT; x++)
        {
            for (int y = (CELL_COUNT - 1); y > 0 ; y--)
            {
                while (tab[x,y] == null && CanFall(x, y))
                {
                    Fall(x, y);
                }
            }
        }
    }

    private bool CanFall(int x, int y)
    {
        for (int i = 0; i < y; i++)
        {
            if (tab[x, i] != null)
                return true;
        }
        return false;
    }

    private void Fall(int x, int y)
    {
        for (int i = y; i > 0; i--)
        {
            tab[x, i] = tab[x, i - 1];
        }
        tab[x, 0] = null;               
    }

    private void ColumnMove()
    {
        for (int _x = 0; _x < CELL_COUNT - 1; _x++)
        {
            while (EmptyColumn(_x) && CanMove(_x))
            {
                MoveColumn(_x);
            }
        }
    }

    private bool EmptyColumn(int x)
    {
        if (tab[x, (CELL_COUNT - 1)] != null)
            return false;
        return true;
    }

    private bool CanMove(int x)
    {
        for (int i = x + 1; i < CELL_COUNT; i++)
        {
            if (!EmptyColumn(i))
                return true;
        }
        return false;
    }

    private void MoveColumn(int _x)
    {
        for (int x = _x; x < CELL_COUNT - 1; x++)
        {
            for (int y = 0; y < CELL_COUNT; y++)
            {
                tab[x, y] = tab[x + 1, y];
            }
        }
        for (int i = 0; i < CELL_COUNT; i++)
        {
            tab[CELL_COUNT - 1, i] = null;
        }
    }

    private bool CheckIfEnd()
    {
        for (int x = 0; x < CELL_COUNT; x++)
        {
            for (int y = 0; y < CELL_COUNT; y++)
            {
                if (tab[x,y] != null)
                {
                    int colIndx = tab[x, y].GetComponent<ButtonScript>().ColorIndex;
                    List<GameObject> list = new List<GameObject>();
                    list.Add(tab[x, y]);
                    CheckNeighbours(x, y, colIndx, list);
                    if (list.Count > 1)
                        return false;
                }
            }
        }
        return true;
    }

    private int CountBlocks()
    {
        int blocks = 0;
        for (int i = 0; i < CELL_COUNT; i++)
        {
            for (int j = 0; j < CELL_COUNT; j++)
            {
                if (tab[i, j] != null)
                    blocks++;
            }
        }
        return blocks;
    }

    public IEnumerator UpdateCoroutine(int x, int y, int colIndex)
    {
        BlockGrid(false);
        List<GameObject> toDestroy = new List<GameObject>();
        toDestroy.Add(tab[x, y]);
        CheckNeighbours(x, y, colIndex, toDestroy);
        if (toDestroy.Count > 1)
        {
            foreach (GameObject gO in toDestroy)
            {
                gO.GetComponent<ButtonScript>().DestroyBut();
            }
            yield return new WaitForSeconds(0.7f);
            BlocksFall();
            ColumnMove();
            inPlay = CountBlocks();
            UpdateGui();
            for (int i = 0; i < CELL_COUNT; i++)
            {
                for (int j = 0; j < CELL_COUNT; j++)
                {
                    if (tab[i, j] != null)
                    {
                        int _x = i;
                        int _y = j;
                        tab[i, j].GetComponent<ButtonScript>().SetIndexes(_x, _y);
                        tab[i, j].GetComponent<ButtonScript>().Move(_x, _y);
                    }
                }
            }
            yield return new WaitForSeconds(0.7f);
        }

        if (CheckIfEnd())
        {
            if (inPlay <= target)
            {
                ShowEndMenu(true);
            }
            else
            {
                ShowEndMenu(false);
            }
        }
        else
        {
            BlockGrid(true);
        }        
    }
}
