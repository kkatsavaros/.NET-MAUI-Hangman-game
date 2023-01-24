using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using Android.Bluetooth;
using Microsoft.Maui.Controls;

namespace HangmanGame;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{

    #region UI Properties
    public string Spotlight
    {
        get => spotlight;
        set
        {
            spotlight = value;
            OnPropertyChanged();              // Implementation method of INotifyPropertyChanged interface.
        }
    }

    public List<char> Letters                 // 2. public field letters which is a List of characters.
    {                                         // 2. Είναι για τα γράμματα που θα γίνουν κουμπιά.
        get => letters;
        set
        {
            letters = value;
            OnPropertyChanged();
        }
    }

    public string Message
    {
        get => message;
        set
        {
            message = value;
            OnPropertyChanged();
        }
    }
    public string GameStatus
    {
        get => gameStatus;
        set
        {
            gameStatus = value;
            OnPropertyChanged();
        }
    }
    public string CurrentImage
    {
        get => currentImage;
        set
        {
            currentImage = value;
            OnPropertyChanged();
        }
    }
    #endregion


    #region Fields
    List<string> words = new List<string>()  {
        "python", "javascript", "maui", "csharp","mongodb", "sql", "xaml",  "word", "excel", "powerpoint", "code", "hotreload","snippets"
     };

    string answer = "";                                  // The word to be guess.
    private string spotlight;
    List<char> guessed = new List<char>();               // Δημιουργώ μία λίστα απο χαρακτήρες.
    private List<char> letters = new List<char>();       // 2. private property και μετά το = we initialize the List.
    private string message;                              // 3. Message: You win!
    int mistakes = 0;
    int maxWrong = 6;
    private string gameStatus;
    private string currentImage = "img0.jpg";
    #endregion


    public MainPage()
    {
        InitializeComponent();
        Letters.AddRange("abcdefghijklmnopqrstuvwxyz");   // ΅We add elements to a List from a String.
        BindingContext = this;
        PickWord();
        CalculateWord(answer, guessed);
    }


    #region Game Engine
    private void PickWord()
    {
        answer = words[new Random().Next(0, words.Count)];  // Range απο 0 μέχρι τον αριθμό των στοιχείων της λίστας.
        Debug.WriteLine("------------------------------------------> " + answer);
    }

    private void CalculateWord(string answer, List<char> guessed)
    {   
        // Ανάλογα με την λέξη που επιλέχθηκε βάζει τα ανάλογα undersdcores.
        var temp = answer.Select(x => (guessed.IndexOf(x) >= 0 ? x : '_')).ToArray();

        Spotlight = string.Join(' ', temp);

    }

    #endregion

    // -----------------------------------------------------------------------------------
    private void Button_Clicked(object sender, EventArgs e)
    {
        var btn = sender as Button; // Declare a variable

        if (btn != null)
        {
            var letter = btn.Text;    // Παίρνω το Text από το κουμπί.
            btn.IsEnabled = false;    // Το κάνω disable.
            HandleGuess(letter[0]);
        }
    }

    private void HandleGuess(char letter)
    {
        
        if (guessed.IndexOf(letter) == -1)   // If the letter has previously selected.
        {
            guessed.Add(letter);
        }

        if (answer.IndexOf(letter) >= 0)     // If the letter is part of the guessed word.
        {
            CalculateWord(answer, guessed);
            CheckIfGameWon();
        }
        else if (answer.IndexOf(letter) == -1)
        {
            mistakes++;
            UpdateStatus();
            CheckIfGameLost();
            CurrentImage = $"img{mistakes}.jpg";
        }
    }


    private void CheckIfGameWon()
    {
        if (Spotlight.Replace(" ", "") == answer)
        {
            Message = "You win!";
            DisableLetters();
        }
        
    }



    private void UpdateStatus()
    {
        GameStatus = $"Errors: {mistakes} of {maxWrong}";
    }

    private void CheckIfGameLost()
    {
        if (mistakes == maxWrong)
        {
            Message = "You Lost!!";
            DisableLetters();
        }
    }

    private void DisableLetters()
    {
        foreach (var children in LettersContainer.Children)
        {
            var btn = children as Button;
            if (btn != null)
            {
                btn.IsEnabled = false;
            }
        }
    }

    private void Reset_Clicked(object sender, EventArgs e)
    {
        mistakes = 0;
        guessed = new List<char>();
        CurrentImage = "img0.jpg";
        PickWord();
        CalculateWord(answer, guessed);
        Message = "";
        UpdateStatus();
        EnableLetters();
    }

    private void EnableLetters()
    {
        foreach (var children in LettersContainer.Children)
        {
            var btn = children as Button;

            if (btn != null)
            {
                btn.IsEnabled = true;
            }
        }
    }
    // -----------------------------------------------------------------------------------

}


