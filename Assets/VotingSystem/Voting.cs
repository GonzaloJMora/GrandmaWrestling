using System.Collections;
using System.Data.Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Wating - 
enum VotingState { Waiting, Voting, Voted, Chaos}
//Waiting - waiting to start the next round of voting
//Voting - in the process of voting for the next chaos
//Voted - chaos is chosen
//Chaos - chaos is currently doing its thing

public class Voting : MonoBehaviour
{
    [Header("Vote Slider Related")]
    [SerializeField] private GameObject votePanel; //canvas with all the sliders
    private int numSliders; //the number of sliders
    private List<Slider> sliders; //the sliders themselves
    private VotingState state; //the current state of voting
    

    [Header("Chaos Related")]
    [SerializeField] private GameObject chaosSystem; //object with all the chaos types
    private Chaos[] ch;
    [SerializeField] private float[] chVoiceLineDelay;
    private int maxChaosSize;
    private int currChaosSize;
    private int[] usedChaosListIndex; //list of already chosen chaos

    [Header("Voting Related")] //See Enum for more details
    [SerializeField] private float waitTime = 15f;
    [SerializeField] private int MaxNumberOfVoteRounds = 15; //Max number of rounds per voting
    [SerializeField] private float votingInterval = 1f; //how long between votes
    [SerializeField] private float chaosTime = 15f;
    [SerializeField] private Color WinninngChaosColor;
    private int currVoteRound = 0;
    private int[] votingChaosIndex;
    private int chosenChaosIndex;
    private float currTime = 0f;

    [Header("Announcer Related")]
    [SerializeField] private GameObject AnnouncerPanel;
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip[] voiceLines;
    [SerializeField] private SoundTicketManager honkAudio;

    [Header("Game Mode Related")]
    [SerializeField] private elimGameMode gamemode;

    [Header("Testing")]
    [SerializeField] private bool OverrideVote = false;
    [SerializeField] private int ChaosOverride;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(new Vector3(0f, 0f, 0f), Chaos.radius);
    }

    // Start is called before the first frame update
    void Awake()
    {
        
        //Voting Related
        Slider[] sd = votePanel.GetComponentsInChildren<Slider>();
        numSliders = sd.Length;
        sliders = new List<Slider>(sd);
        ResetSliders();
        //Debug.Log("numSliders " + numSliders);

        //Chaos Related
        Component[] comp = chaosSystem.GetComponents(typeof(Chaos));
        ch = new Chaos[comp.Length];
        for(int i = 0; i < comp.Length; i += 1)
        {
            ch[i] = (Chaos)comp[i];
        }
        maxChaosSize = ch.Length;
        currChaosSize = maxChaosSize;
        usedChaosListIndex = new int[maxChaosSize];
        ResetArray(usedChaosListIndex);

        //Voting Related
        votingChaosIndex = new int[numSliders];
        ResetArray(votingChaosIndex);

        //Announcer Related
        ToggleAnnouncer(false);
        //Debug.Log("currChaosSize: " + currChaosSize);
    }

    private void ResetArray(int[] a)
    {
        for(int i = 0; i < a.Length; i += 1)
        {
            a[i] = -1;
        }
    }

    private void PrintUsedChaosIndex()
    {
        for (int i = 0; i < usedChaosListIndex.Length; i += 1)
        {
            if(usedChaosListIndex[i] == -1) { continue; }
            //Debug.Log(ch[usedChaosListIndex[i]]);
        }
    }



    // Update is called once per frame
    void Update()
    {
        //Debug.Log("currChaosSize: " + currChaosSize);
        //Debug.Log("Inside of ChooseVotingChaos");

        if (gamemode.isGameOver) {
                //ch[chosenChaosIndex].Stop();
                ToggleSliderPanel(false);
                ToggleAnnouncer(false);
                ResetSliders();
                ResetArray(usedChaosListIndex);
                ResetArray(votingChaosIndex);
                currChaosSize = maxChaosSize;
                currTime = 0f;
                currVoteRound = 0;
                ChangeState(VotingState.Waiting);
        }

        if (!gamemode.canStartVoting && state == VotingState.Waiting) {
            return;
        }

        if (OverrideVote && state == VotingState.Voted)
        {
            chosenChaosIndex = ChaosOverride;
            //state = VotingState.Chaos;

            if (chosenChaosIndex == 7) {
                honkAudio.playSound();
            }
            else {
                audio.PlayOneShot(voiceLines[chosenChaosIndex]);
            }

            Invoke("startChaos", chVoiceLineDelay[chosenChaosIndex]);
            //startChaos();
            //ch[chosenChaosIndex].Trigger();
            state = VotingState.Chaos;
            return;
        }

        currTime += Time.deltaTime;

        if(state == VotingState.Waiting)
        {
            ToggleSliderPanel(false);
            if (currTime >= waitTime)
            {
                currTime = 0f;
                ChooseVotingChaos();
                ChangeSliderName();
                ChangeState(VotingState.Voting);
            }
        }
        else if(state == VotingState.Voting)
        {
            ToggleSliderPanel(true);
            if (currVoteRound < MaxNumberOfVoteRounds)
            {
                //Debug.Log("currVoteRound" + currVoteRound);
                if(currTime > votingInterval)
                {
                    VoteOnChaos();
                    currVoteRound += 1;
                    currTime = 0f;
                }
            }
            else
            {
                //returns the index of sliders with the highest value
                int index = GetHighestRatedChaos();
                
                ChangeChosenChaosColor(index);
                
                chosenChaosIndex = votingChaosIndex[index];
                ToggleAnnouncer(true);

                currVoteRound = 0;
                ChangeState(VotingState.Voted);
                currTime = 0f;
            }

        }
        else if(state == VotingState.Voted)
        {
            AddIndex(usedChaosListIndex, chosenChaosIndex);
            
            currChaosSize -= 1;
            
            PrintUsedChaosIndex();
            ResetArray(votingChaosIndex);

            if (chosenChaosIndex == 7) {
                honkAudio.playSound();
            }
            else {
                audio.PlayOneShot(voiceLines[chosenChaosIndex]);
            }

            ChangeState(VotingState.Chaos);
            Invoke("startChaos", chVoiceLineDelay[chosenChaosIndex]);
            //startChaos();
            // ch[chosenChaosIndex].Trigger();
            // ChangeState(VotingState.Chaos);
            // currTime = 0f;
        }
        else if (state == VotingState.Chaos)
        {
            if(currTime > chaosTime/4f)
            {
                ToggleSliderPanel(false);
                ResetSliders();
                ToggleAnnouncer(false);
            }

            if(currTime >= chaosTime)
            {
                currTime = 0f;
                ch[chosenChaosIndex].Stop();
                ChangeState(VotingState.Waiting);
            }
        }

    }

    private void startChaos() {
        ch[chosenChaosIndex].Trigger();
        currTime = 0f;
    }

    private void ChangeSliderName()
    {
        for (int i = 0; i < numSliders; i += 1)
        {
            TMP_Text sd = sliders[i].GetComponentInChildren<TMP_Text>();
            sd.text = ch[votingChaosIndex[i]].GetName();
        }
    }

    private void ChangeChosenChaosColor(int index)
    {
        Image[] a = sliders[index].gameObject.GetComponentsInChildren<Image>();
        for(int i = 0; i < a.Length; i += 1)
        {
            GameObject obj = a[i].gameObject;
            //Debug.Log(obj.name + " " + i);
            a[i].color = WinninngChaosColor;
        }
    }    
    private void ResetSliders()
    {
        for(int i = 0; i < numSliders; i += 1)
        {
            sliders[i].value = 0;
            sliders[i].maxValue = 1;
            
            TMP_Text sd = sliders[i].GetComponentInChildren<TMP_Text>();
            sd.text = "";
            Image[] a = sliders[i].gameObject.GetComponentsInChildren<Image>();
            for(int j = 0; j < a.Length; j += 1)
            {
                a[j].color = Color.white;
            }
        }
    }

    private void ChooseVotingChaos()
    {
        currChaosSize -= 1;
        //if number of chaos left is < the numbers of sliders
        if(currChaosSize < numSliders)
        {
            //Debug.Log("Reseting - size" + currChaosSize);
            
            //reset them
            ResetArray(usedChaosListIndex);
            AddIndex(usedChaosListIndex, chosenChaosIndex);
            currChaosSize = maxChaosSize - 1;
        }
        
        //Debug.Log("Picking random index");
        
        //pick random chaos
        for (int i = 0; i < numSliders; i += 1)
        {
            int chaosIndex = Random.Range(0, maxChaosSize);
            
            //Debug.Log("index: " + chaosIndex);
            //Debug.Log("Element: " + ch[chaosIndex]);
            
            //redo if chaos was already chosen
            if(FindIndexof(votingChaosIndex, chaosIndex) != -1) { i -= 1; continue; }
            if(FindIndexof(usedChaosListIndex, chaosIndex) != -1) { i -= 1; continue; }
            
            //add if index is valid
            AddIndex(votingChaosIndex, chaosIndex);
        }
    }

    private int FindIndexof(int[] a, int index)
    {
        for(int i = 0; i < a.Length; i += 1)
        {
            if(a[i] == index)
            {
                return i;
            }
        }
        return -1; //did not find
    }
    private void AddIndex(int[] a, int index)
    {
        for (int i = 0; i < a.Length; i += 1)
        {
            if (a[i] == -1)
            {
                a[i] = index;
                return;
            }
        }
    }

    private void VoteOnChaos()
    {
        //Debug.Log("inside of VoteOnChaos");
        
        //generate a random number of votes;
        int currVotes = Random.Range(5, 20);
        
        //Debug.Log("votes to add: " + currVotes);
        
        //randomly pick a slider to add votes to
        int sliderIndex = Random.Range(0, numSliders);
        
        //Debug.Log("slider to add to: " + sliderIndex);
        
        //change the values of the sliders
        for (int i = 0; i < numSliders; i += 1)
        {
            if (sliderIndex == i)
            {
                sliders[i].value += currVotes;
            }
            else
            {
                sliders[i].value += currVotes / Random.Range(2, 4);
            }
            sliders[i].maxValue += currVotes;
        }
    }

    private int GetHighestRatedChaos()
    {
        //Debug.Log("Inside of GetHighestRatedChaos");
        
        //find highest voted chaos
        int index = -1;
        float max = 0;
        
        //set that as the chosen chaos
        for (int i = 0; i < numSliders; i += 1)
        {
            //Debug.Log("i: " + i + " value: " + sliders[i].value);
            
            if (sliders[i].value > max)
            {
                max = sliders[i].value;
                index = i;
            }
        }

        //Debug.Log("Index is" + index);

        return index;
    }

    private void ChangeState(VotingState state)
    {
        //Debug.Log("Switching from " + this.state + " to " + state);
        this.state = state;
    }

    private void ToggleSliderPanel(bool isVisble)
    {
        votePanel.SetActive(isVisble);
    }

    private void ToggleAnnouncer(bool isVisible)
    {
        /*if(isVisible)
        {
            TMP_Text mp = AnnouncerPanel.GetComponentInChildren<TMP_Text>();
            mp.text = ch[chosenChaosIndex].GetOneliner();
            //ch[chosenChaosIndex].PlayAnnouncerClip();
        }*/
        AnnouncerPanel.SetActive(isVisible);

    }
    
}
