
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ObjectGeneration
{
    public class ObjectGeneratorMain : MonoBehaviour
    {
        public static ObjectGeneratorMain instance { get; private set; }
        public Action SetupObjectGeneration;

        //Fields and Properties
        [field: SerializeField] List<GameObject> spreadedObject = new List<GameObject>();
        [field: SerializeField] private int numberSpreadedObject { get; set; }
        [field: SerializeField] private Vector2 startSpreadedObjectPos { get; set; }
        [field: SerializeField] private Vector2 endSpreadedObjectPos { get; set; }
        [field: SerializeField, Range(1, 100)] int spreadGapMin { get; set; }
        [field: SerializeField, Range(1, 100)] int spreadGapMax { get; set; }

        private void OnEnable()
        {
            if (instance != null)
            {
                return;
            }
            instance = this;
            DontDestroyOnLoad(this);
        }

        // Start is called before the first frame update
        void Start()
        {
            SetupObjectGeneration += GenerateObjectPositions(() => { Debug.Log("Object Generation has completed"); });
            Debug.Log("Started Object Generator0");
        }

        void Initialize()
        {
            
        }

        /// <summary>
        /// Return an event after complete generating objects
        /// </summary>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        Action GenerateObjectPositions(Action onCompleted)
        {
            for (int i = (int)startSpreadedObjectPos.y; i < (int)endSpreadedObjectPos.y; i++)
            {
                for (int j = (int)startSpreadedObjectPos.x + Random.Range(spreadGapMin,spreadGapMax); j < (int)endSpreadedObjectPos.x; j++)
                {
                    if (numberSpreadedObject <= 0)
                    {
                        break;
                    }
                    var randomSelectedObject = Random.Range(0, spreadedObject.Count);
                    var instantiatedObject = Instantiate(spreadedObject[randomSelectedObject], position: new Vector3(j, 0, i), rotation: Quaternion.identity);
                    Debug.LogFormat("{0} has spawned at {1}", instantiatedObject.name, new Vector3(j, 0, i));
                    Debug.Log(numberSpreadedObject);
                    numberSpreadedObject--;
                }
            }

            return () => onCompleted();
        }
    }
}
