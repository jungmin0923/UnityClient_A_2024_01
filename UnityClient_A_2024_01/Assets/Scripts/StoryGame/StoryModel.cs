using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STORYGAME;

[CreateAssetMenu(fileName = "NewStory", menuName = "ScriptableObjects/StoryModel")]
public class StoryModel : MonoBehaviour
{
    public int storyNumber;         //스토리 번호
    public Texture2D MainImage;     //스토리 보여줄 이미지 텍스쳐

    public enum STORYTYPE           //스토리 타입 설정
    {
        MAIN,
        SUB,
        SERIAL
    }

    public STORYTYPE storytype;      //스토리 타입 선언
    public bool storyDone;           //스토리 종료 여부

    [TextArea(10, 10)]      //텍스트 영역 표시
    public string storyText;          //메인 스토리

    public Option[] options;        //선택지 배열

    [System.Serializable]
    public class Option
    {
        public string optionText;
        public string buttonText;
        public EventCheck eventCheck;
    }

    [System.Serializable]
    public class EventCheck
    {
        public int checkValue;
        public enum EventType : int
        {
            NONE,
            GoToBattle,
            CheckSTR,
            CheckDEX,
            CheckCON,
            CheckINT,
            CheckWIS,
            CheckCHA
        }

        public EventType eventType;

        public Result[] suceessResult;
        public Result[] failResult;
    }

    [System.Serializable]
    public class Result
    {
       public enum ResultType : int
        {
            ChangeHp,
            ChangeSp,
            AddExperience,
            GoToShop,
            GoToNextStory,
            GoToRandomStory,
            GoToEnding
        }

        public ResultType resultType;
        public int value;
        public Stats stats;
    }

}
