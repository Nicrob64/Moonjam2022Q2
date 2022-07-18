using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OfficeSceneController : MonoBehaviour
{

    struct Prompt{
        string question;
        bool corpa;
    }

    public QTEScript QTE;

    public int questionsRemaining = 10;
    public float stockPrice = 100;
    public float unionStrength = 0;

    public int balance = 0;

    public Text prompt;
    public Text stats;

    int currentSession = 0;

    public Cutscene qteCutscene;

    bool isQuestionCorpa = false;

    public List<string> corpoq = new List<string>() 
    {
        "Profits on all lubrication-based products increase by 0.69%. Attainment Chamber staff will be required to work an additional 45 minutes per shift.",
        "Staff retention increases by 1%. Worker morale decreases by 1%",
        "A small delay to some smaller packages will occur so they can be shipped alongside other orders to cut the cost of transportation. Transport Warehouses will be fuller, with less accessibility for staff.",
        "We can save money on our utilities bills by reducing the amount of usage. All staff gets one bathroom break revoked.",
        "Purchase local competition so The People realise they have to choose us. The local populace will suffer from unemployment",
        "Transfer labour to automated workers, replacing Bio-Drones over time.",
        "Allow workers to urinate in a portable container when required, to save the untold amount of minutes that are lost to traversing to and from the nearest bathroom"
    };

    public List<string> eatTheRich = new List<string>()
    {
        "Give staff an additional 5 minutes of break time per shift. Profits will plummet.",
        "Give all workers the chance to apply for health insurance. All CEOÅfs bonuses will nose-dive by at least 0.01%",
        "Improve warehouse wellbeing and employ an overseer for Health and Safety. All management lose one of their allotted 20 weeks vacation.",
        "Provide minimum wage. Prepare to cope with a huge amount of staff absences as they go spending their newfound wealth on drugs and prostitutes.",
        "Change to a more environmentally friendly packaging and delivery solution. At least one local manager will be shot."
    };

    bool canAnswer = false;

    public void DoQuestion()
    {
        canAnswer = true;
        int q = UnityEngine.Random.Range(0, eatTheRich.Count + corpoq.Count);
        if(q < corpoq.Count)
        {
            isQuestionCorpa = true;
            prompt.text = corpoq[q];
        }
        else
        {
            q -= corpoq.Count;
            prompt.text = eatTheRich[q];
        }
    }

    public void AnswerQ(bool yes)
    {
        if (!canAnswer)
            return;
        canAnswer = false;
        if (isQuestionCorpa)
        {
            if (yes)
            {
                balance++;
                stockPrice += Random.Range(4.6f, 23.5f);
            }
            else
            {
                balance--;
                unionStrength += Random.Range(4.6f, 23.5f);
            }
        }
        else
        {
            if (yes)
            {
                balance--;
                unionStrength += Random.Range(4.6f, 23.5f);
            }
            else
            {
                balance++;
                stockPrice += Random.Range(4.6f, 23.5f);
            }
        }

        prompt.text = "QTE!!!!!!!!";
        questionsRemaining--;
        stats.text = string.Format("STOCK PRICE: ${0:0.00}\nUNION STRENGTH: {1:0.00}%\nDECISIONS REMAINING: {2}", stockPrice, unionStrength, questionsRemaining);

        if(questionsRemaining == 0)
        {
            if(balance > 0)
            {
                SceneTransitionHelper.Instance.TransitionReason = TransitionReason.EndingCorpo;
                SceneTransitionHelper.Instance.FromScene = FromScene.Management;
                SceneManager.LoadScene("Ending");
            }
            else
            {
                SceneTransitionHelper.Instance.TransitionReason = TransitionReason.EndingUnion;
                SceneTransitionHelper.Instance.FromScene = FromScene.Management;
                SceneManager.LoadScene("Ending");
            }
        }

        qteCutscene.Reset();
        qteCutscene.Play();
    }

    public void DoQTESession()
    {
        QTE.Reset();

        if(currentSession == 0)
        {
            QTE.AddButtonPrompt(KeyCode.B, 1f);
            QTE.AddButtonPrompt(KeyCode.A, 1f);
            QTE.AddButtonPrompt(KeyCode.L, 1f);
            QTE.AddButtonPrompt(KeyCode.D, 1f);
            QTE.AddButtonPrompt(KeyCode.At, 1f);
        }

        if (currentSession == 1)
        {

            QTE.AddButtonPrompt(KeyCode.LeftBracket, 0.5f);
            QTE.AddButtonPrompt(KeyCode.Alpha1, 0.5f);
            QTE.AddButtonPrompt(KeyCode.Alpha5, 0.5f);
            QTE.AddButtonPrompt(KeyCode.H, 0.5f);
            QTE.AddButtonPrompt(KeyCode.LeftCurlyBracket, 1f);
        }

        if(currentSession == 2)
        {
            
            QTE.AddButtonPrompt(KeyCode.UpArrow, 0.5f);
            QTE.AddButtonPrompt(KeyCode.Comma, 0.5f);
            QTE.AddButtonPrompt(KeyCode.F, 0.5f);
            QTE.AddButtonPrompt(KeyCode.L, 1.0f);
            QTE.AddButtonPrompt(KeyCode.K, 0.5f);
            QTE.AddButtonPrompt(KeyCode.Equals, 0.5f);
            QTE.AddButtonPrompt(KeyCode.B, 0.5f);
            QTE.AddButtonPrompt(KeyCode.A, 0.5f);
            QTE.AddButtonPrompt(KeyCode.Return, 0.5f);
        }

        if (currentSession == 3)
        {
            QTE.AddButtonPrompt(KeyCode.UpArrow, 0.5f);
            QTE.AddButtonPrompt(KeyCode.UpArrow, 0.5f);
            QTE.AddButtonPrompt(KeyCode.DownArrow, 0.5f);
            QTE.AddButtonPrompt(KeyCode.Hash, 1.0f);
            QTE.AddButtonPrompt(KeyCode.RightArrow, 0.5f);
            QTE.AddButtonPrompt(KeyCode.Equals, 0.5f);
            QTE.AddButtonPrompt(KeyCode.B, 0.5f);
            QTE.AddButtonPrompt(KeyCode.A, 0.5f);
            QTE.AddButtonPrompt(KeyCode.Return, 0.5f);
        }

        if (currentSession == 4)
        {
            QTE.AddButtonPrompt(KeyCode.UpArrow, 0.5f);
            QTE.AddButtonPrompt(KeyCode.UpArrow, 0.5f);
            QTE.AddButtonPrompt(KeyCode.DownArrow, 0.5f);
            QTE.AddButtonPrompt(KeyCode.Hash, 1.0f);
            QTE.AddButtonPrompt(KeyCode.RightArrow, 0.5f);
            QTE.AddButtonPrompt(KeyCode.Equals, 0.5f);
            QTE.AddButtonPrompt(KeyCode.B, 0.5f);
            QTE.AddButtonPrompt(KeyCode.A, 0.5f);
            QTE.AddButtonPrompt(KeyCode.Return, 0.5f);
        }


        if (currentSession == 5)
        {
            QTE.AddButtonPrompt(KeyCode.D, 0.5f);
            QTE.AddButtonPrompt(KeyCode.O, 0.5f);
            QTE.AddButtonPrompt(KeyCode.Q, 0.5f);
            QTE.AddButtonPrompt(KeyCode.V, 1.0f);
            QTE.AddButtonPrompt(KeyCode.Semicolon, 0.5f);
            QTE.AddButtonPrompt(KeyCode.Alpha8, 0.5f);
            QTE.AddButtonPrompt(KeyCode.J, 0.5f);
            QTE.AddButtonPrompt(KeyCode.B, 0.5f);
            QTE.AddButtonPrompt(KeyCode.Return, 0.5f);
        }
        if (currentSession == 6)
        {
            QTE.AddButtonPrompt(KeyCode.D, 0.5f);
            QTE.AddButtonPrompt(KeyCode.O, 0.5f);
            QTE.AddButtonPrompt(KeyCode.Q, 0.5f);
            QTE.AddButtonPrompt(KeyCode.V, 1.0f);
            QTE.AddButtonPrompt(KeyCode.Semicolon, 0.5f);
            QTE.AddButtonPrompt(KeyCode.Alpha8, 0.5f);
            QTE.AddButtonPrompt(KeyCode.J, 0.5f);
            QTE.AddButtonPrompt(KeyCode.B, 0.5f);
            QTE.AddButtonPrompt(KeyCode.Return, 0.5f);
        }
        if (currentSession == 7)
        {
            QTE.AddButtonPrompt(KeyCode.I, 0.5f);
            QTE.AddButtonPrompt(KeyCode.C, 0.5f);
            QTE.AddButtonPrompt(KeyCode.B, 0.5f);
            QTE.AddButtonPrompt(KeyCode.I, 1.0f);
            QTE.AddButtonPrompt(KeyCode.N, 0.5f);
            QTE.AddButtonPrompt(KeyCode.B, 0.5f);
        }
        if (currentSession == 8)
        {
            QTE.AddButtonPrompt(KeyCode.Z, 0.5f);
            QTE.AddButtonPrompt(KeyCode.E, 0.5f);
            QTE.AddButtonPrompt(KeyCode.A, 0.5f);
            QTE.AddButtonPrompt(KeyCode.L, 1.0f);
            QTE.AddButtonPrompt(KeyCode.O, 0.5f);
            QTE.AddButtonPrompt(KeyCode.T, 0.5f);
        }
        if (currentSession == 9)
        {
            QTE.AddButtonPrompt(KeyCode.B, 0.5f);
            QTE.AddButtonPrompt(KeyCode.A, 0.5f);
            QTE.AddButtonPrompt(KeyCode.A, 0.5f);
            QTE.AddButtonPrompt(KeyCode.A, 1.0f);
            QTE.AddButtonPrompt(KeyCode.A, 0.5f);
            QTE.AddButtonPrompt(KeyCode.L, 0.5f);
            QTE.AddButtonPrompt(KeyCode.D, 0.5f);
        }

        currentSession++;
        QTE.Activate(3);

    }

}
