using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;          //������
using System.Text;          //�ؽ�Ʈ ���
using UnityEngine.UI;       //UI ����ϱ�����
using TMPro;

namespace STORYGAME //�̸� �浹 ����
{
#if UNITY_EDITOR
    [CustomEditor(typeof(GameSystem))]
    public class GameSystemEditor : Editor              //����Ƽ �����͸� ���
    {
        public static GameSystem instance;
        public override void OnInspectorGUI()               //�ν����� �Լ��� ������
        {
            base.OnInspectorGUI();                          //���� �ν����͸� �����ͼ� ����

            GameSystem gameSystem = (GameSystem)target;     //���� �ý��� ��ũ��Ʈ Ÿ���� ����

            if(GUILayout.Button("Reset Story Models"))          //��ư�� ����
            {
                gameSystem.ResetStoryModels();
            }

            if (GUILayout.Button("Assing Text Component by Name"))          //��ư�� ���� (UI ������Ʈ�� �ҷ��´�)
            {
                //������Ʈ �̸����� Text ������Ʈ ã��
                GameObject textObject = GameObject.Find("StoryTextUI");
                if (textObject != null)
                {
                    Text textComponent = textObject.GetComponent<Text>();
                    if(textComponent != null)
                    {
                        //Text Component �Ҵ�
                        gameSystem.textComponent = textComponent;
                        Debug.Log("Text Component assigned Successfully");
                    }
                }
            }
        }
    }
#endif

    public class GameSystem : MonoBehaviour
    {
        public static GameSystem instance;              //������ ��Ŭ�� ȭ
        public Text textComponent = null;

        public float delay = 0.1f;                      //�� ���ڰ� ��Ÿ���� �� �ɸ��� �ð�
        public string fullText;                         //��ü ǥ���� �ؽ�Ʈ
        public string currentText = "";                 //������� ǥ�õ� �ؽ�Ʈ

        public enum GAMESTATE                           //���°� ���� ������
        {
            STORYSHOW,
            WAITSELECT,
            STROTYEND,
            ENDMODE
        }

        public GAMESTATE currentState;
        public StoryTableObject[] storyModels;          //������ �ִ��� �𵨵� �ҽ��ڵ� ��ġ �̵�
        public StoryTableObject currentModles;          //���� ���丮 �� ��ü
        public int currentStoryIndex;                   //���丮 �� �ε���
        public bool showStory = false;
        
        private void Awake()
        {
            instance = this;
        }

        public void Start()                     //���� ���۽�
        {
            StartCoroutine(ShowText());         //�ؽ�Ʈ�� �����ش�
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q)) StoryShow(1);              //Q Ű�� ������ 1�� ���丮
            if (Input.GetKeyDown(KeyCode.W)) StoryShow(2);              //W Ű�� ������ 2�� ���丮
            if (Input.GetKeyDown(KeyCode.E)) StoryShow(3);              //E Ű�� ������ 3�� ���丮

            if (Input.GetKeyDown(KeyCode.Space))
            {
                delay = 0.0f;
            }
        }

        public void StoryShow(int number)                               
        {
            if (!showStory)
            {
                currentModles = FindStoryModel(number);                     //���丮 ���� ��ȣ�� ã�Ƽ�
                delay = 0.1f;
                StartCoroutine(ShowText());                                 //��ƾ�� ���� ��Ų��.
            }
        }
        StoryTableObject FindStoryModel(int number)                 //���丮 �� ��ȣ�� ã�� ����
        {
            StoryTableObject tempStoryModels = null;                //temp �̸� ���� �س��� ������ ����
            for(int i = 0; i < storyModels.Length; i++)             //��ư���� �޾ƿ� ����Ʈ�� for������ �˻��Ͽ�
            {
                if(storyModels[i].storyNumber == number)            //���ڰ� ���� ���
                {
                    tempStoryModels = storyModels[i];               //�̸� ������ ���� ������ �ְ�
                    break;                                          //for ���� ���� ���´�
                }
            }

            return tempStoryModels;                                 //���丮 ���� �����ش�
        }

        IEnumerator ShowText()
        {
            showStory = true;
            for (int i = 0; i <= currentModles.storyText.Length; i++)
            {
                currentText = currentModles.storyText.Substring(0, i);
                textComponent.text = currentText;
                yield return new WaitForSeconds(delay);
            }

            yield return new WaitForSeconds(delay);
            showStory = false;
        }
       
#if UNITY_EDITOR
        [ContextMenu("Reset Story Models")]

        public void ResetStoryModels()
        {
            storyModels = Resources.LoadAll<StoryTableObject>("");// Resources ���� �Ʒ� ��� StoryModel �ҷ� ����
        }
#endif

    }
}