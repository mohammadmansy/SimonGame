using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CircleOrderGame : MonoBehaviour
{
    public Button redButton;
    public Button blueButton;
    public Button greenButton;
    public Button yellowButton;
    public Button startButton;

    public TextMeshProUGUI instructionsText;

    //private int hardness = 3;
    private List<Button> buttons = new List<Button>();
    private List<string> colors = new List<string> { "Red", "Blue", "Green", "Yellow" };
    private List<string> correctOrder = new List<string>();
    private List<string> playerOrder = new List<string>();
    private float displayTime = 1f;

    public AudioSource RightAnswer;
    public AudioSource WrongAnswer;

    public AudioClip buttonClickSound;
    private AudioSource audioSource;
    bool isCorrect = true;

    void Start()
    {
        buttons.Add(redButton);
        buttons.Add(blueButton);
        buttons.Add(greenButton);
        buttons.Add(yellowButton);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = buttonClickSound;

        redButton.onClick.AddListener(() => CircleClicked("Red", redButton));
        blueButton.onClick.AddListener(() => CircleClicked("Blue", blueButton));
        greenButton.onClick.AddListener(() => CircleClicked("Green", greenButton));
        yellowButton.onClick.AddListener(() => CircleClicked("Yellow", yellowButton));

        startButton.onClick.AddListener(() => {
            startButton.interactable = false;
            RestartGame();
        });
    }

    IEnumerator DisplayOrder()
    {
        yield return new WaitForSeconds(2f);

        if (isCorrect)
            correctOrder.Add(GetRandomColor());

        //instructionsText.text = "احفظ ترتيب الألوان";

        foreach (string color in correctOrder)
        {
            Button button = GetButtonByColor(color);
            if (button != null)
            {
                HighlightButton(button);
                yield return new WaitForSeconds(displayTime);
            }
        }

        //instructionsText.text = "كرر ترتيب الألوان";
        playerOrder.Clear();
    }

    void HighlightButton(Button button)
    {
        StartCoroutine(HighlightCoroutine(button));
    }

    void HighlightClickedButton(Button button)
    {
        StartCoroutine(HighlightedClick(button));
    }

    IEnumerator HighlightedClick(Button button)
    {
        button.interactable = false;

        Color originalColor = button.image.color;
        button.image.color = GetIntenseColor(button.name); 
        yield return new WaitForSeconds(0.2f); 
        button.image.color = originalColor;

        button.interactable = true;
    }
    IEnumerator HighlightCoroutine(Button button)
    {
        Color originalColor = button.image.color;
        Vector3 originalScale = button.transform.localScale;

        button.image.color = GetIntenseColor(button.name); // Make the color more intense
        button.transform.localScale = originalScale * 1.2f; // Increase the size of the button

        yield return new WaitForSeconds(0.5f); // Wait for half a second

        button.image.color = originalColor; // Return to the original color
        button.transform.localScale = originalScale; // Return to the original size
    }

    void CircleClicked(string color, Button button)
    {
        audioSource.Play();

        playerOrder.Add(color);
        HighlightClickedButton(button);

        if (playerOrder.Count == correctOrder.Count)
        {
            CheckOrder();
        }
    }

    void CheckOrder()
    {
        isCorrect = true;

        for (int i = 0; i < correctOrder.Count; i++)
        {
            if (playerOrder[i] != correctOrder[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            Debug.Log("Correct Order!");
            instructionsText.text = "Good Job";
            //hardness++;
            RightAnswer.Play();
            playerOrder.Clear();

            StartCoroutine(DisplayOrder()); // Show the order again with an extra color

        }
        else
        {
            Debug.Log("Incorrect Order!");
            instructionsText.text = "Wrong Answer";
            WrongAnswer.Play();
            startButton.interactable = true;

        }


    }
    
    string GetRandomColor()
    {
        int index = Random.Range(0, colors.Count);
        return colors[index];
    }

    Button GetButtonByColor(string color)
    {
        switch (color)
        {
            case "Red": return redButton;
            case "Blue": return blueButton;
            case "Green": return greenButton;
            case "Yellow": return yellowButton;
            default: return null;
        }
    }

    Color GetColorByName(string color)
    {
        switch (color)
        {
            case "Red": return Color.red;
            case "Blue": return Color.blue;
            case "Green": return Color.green;
            case "Yellow": return Color.yellow;
            default: return Color.white;
        }
    }

    Color GetIntenseColor(string color)
    {
        switch (color)
        {
            case "Red": return new Color(1.0f, 0.0f, 0.0f); // Bright Red
            case "Blue": return new Color(0.0f, 0.0f, 1.0f); // Bright Blue
            case "Green": return new Color(0.0f, 1.0f, 0.0f); // Bright Green
            case "Yellow": return new Color(1.0f, 1.0f, 0.0f); // Bright Yellow
            default: return Color.white;
        }
    }
    void RestartGame()
    {
        correctOrder.Clear(); // Clear the correct order to start from the beginning
        playerOrder.Clear();
        isCorrect = true;
        StartCoroutine(DisplayOrder()); // Start the display from the beginning
    }
}
