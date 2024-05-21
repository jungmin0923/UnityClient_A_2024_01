using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;               //UI를 컨트롤 할 것이라 추가
using System;                       //String 관련 함수 사용 하기 위해 추가

public class StorySystem : MonoBehaviour
{
    public static StorySystem instance;                 //간단한 싱글톤 화
    public StoryModel currentStoryModel;                //지금 스토리 모델 참조

    public enum TEXTSYSTEM
    {
        DOING,
        SELECT, 
        NONE
    }

    public float delay = 0.1f;                          //각 글자가 나타나는데 걸리는 시간
    public string fullText;                             //전체 표시할 텍스트
    private string currentText = "";                    //현재까지 표시된 텍스트
    public Text textComponent;                          //text 컴포넌트
    public Text storyIndex;                             //스토리 번호 표시할 UI
    public Image imageComponent;                        //이미지 UI

    public Button[] buttonWay = new Button[3];          //선택지 버튼 추가
    public Text[] buttonWayText = new Text[3];          //선택지 버튼 Text

    public TEXTSYSTEM currentTextShow = TEXTSYSTEM.NONE;    
                                                        
        

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < buttonWay.Length; i++)
        {
            int wayIndex = i;
            buttonWay[i].onClick.AddListener(() => OnWayClick(wayIndex));
        }

        CoShowText();
    }

    public void StoryModeIInit()
    {
        fullText = currentStoryModel.storyText;
        storyIndex.text = currentStoryModel.storyNumber.ToString();

        for (int i = 0; i < currentStoryModel.options.Length; i++)
        {
            buttonWayText[i].text = currentStoryModel.options[i].buttonText;            //버튼 이름을 설정
        }
    }

    public void OnWayClick(int index)           //선택지버튼에 따른 함수 index는 버튼에 연결된 번호를 받아온다.
    {
        if (currentTextShow == TEXTSYSTEM.DOING)
            return;

        bool CheckEventTypeNone = false;        //기본으로 NONE일때는 무조건 성공이라고 판단하고 실패시에 다시 불리는것을 피하기 위해서 Bool 선언
        StoryModel playStoryModel = currentStoryModel;

        if (playStoryModel.options[index].eventCheck.eventType == StoryModel.EventCheck.EventType.NONE) //버튼에서 체크할 이벤트 타입이 없으면 성공으로 간주
        {
            for(int i = 0; i < playStoryModel.options[index].eventCheck.suceessResult.Length; i++)  //성공시 결과 이벤트 설정한것들을 동작하게 한다.
            {
                GameSystem.instance.ApplyChoice(currentStoryModel.options[index].eventCheck.suceessResult[i]);
                CheckEventTypeNone = true;
            }
        }
    }

    public void CoShowText()            //전체저인 스토리 모델 호출
    {
        StoryModeIInit();               //스토리 모델을 셋팅
        ResetShow();                    //창 화면을 초기화
        StartCoroutine(ShowText());     //연출 진행
    }

    public void ResetShow()      //스토리 다 보여주고 초기화
    {
        textComponent.text = "";        //보여진 텍스트 빈칸

        for(int i = 0; i < buttonWay.Length; i++)   //버튼들 다시 가리기
        {
            buttonWay[i].gameObject.SetActive(false);
        }
    }

    IEnumerator ShowText()                                          //코루틴 함수 사용
    {
        currentTextShow = TEXTSYSTEM.DOING;

        if(currentStoryModel.MainImage != null)
        {
            //Texture2D를 Sprtie 변환
            Rect rect = new Rect(0, 0, currentStoryModel.MainImage.width, currentStoryModel.MainImage.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Sprite sprite = Sprite.Create(currentStoryModel.MainImage, rect, pivot);

            imageComponent.sprite = sprite;
        }
        else
        {
            Debug.LogError("텍스쳐 로딩이 되지 않습니다. : " + currentStoryModel.MainImage.name);
        }
        for(int i = 0; i < fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);                 //Substring 문자열 자르는 함수
            textComponent.text = currentText;
            yield return new WaitForSeconds(delay);                 //delay 초만큼 For 문을 지연 시킨다.
        }

        for(int i = 0; i < currentStoryModel.options.Length; i++)
        {
            buttonWay[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(delay);

        currentTextShow = TEXTSYSTEM.NONE;
    }

}
