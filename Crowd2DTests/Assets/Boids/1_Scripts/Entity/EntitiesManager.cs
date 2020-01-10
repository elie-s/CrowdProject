using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EANS.Flocks
{
    public class EntitiesManager : MonoBehaviour
    {
        //[SerializeField] private List<EntityData> entities = new List<EntityData>();
        [SerializeField] private EntitiesData data = default;
        [SerializeField] private Transform entitiesParent = default;
        [SerializeField] private GameObject entityPrefab = default;
        [SerializeField] private Camera mainCamera = default;
        [SerializeField] private EntitySettings settings = default;
        [SerializeField] private CameraController camControl = default;
        [Header("Initialization")]
        [SerializeField] private int entitiesCountOnStart = 100;
        [SerializeField] private float spawnRadius = 20.0f;
        [SerializeField] private Transform[] anchors = default;
        private int anchorIndex = 0;
        private void Awake()
        {
            data.entities = new List<EntityData>();
            SpawnOnStart();
        }

        private void Start()
        {
            StartCoroutine(DelayedLoopRoutine());
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0)) SpawnOnMouse();
            SetCameraLookAtPosition();

            if (Input.GetKeyDown(KeyCode.Escape)) Quit();
        }

        private void Quit()
        {
            Application.Quit();
        }

        private void OnApplicationQuit()
        {
            data.entities.Clear();
        }

        private IEnumerator DelayedLoopRoutine()
        {
            ManageEntities();

            yield return new WaitForSeconds(0.1f);

            StartCoroutine(DelayedLoopRoutine());
        }

        private void SetCameraLookAtPosition()
        {
            Vector2 position = Vector2.zero;

            foreach (EntityData entity in data.entities)
            {
                position += (Vector2)entity.position;
            }

            position /= data.entities.Count;


            camControl.LookAt(Vector2.Distance(position, data.predatorPosition) > 30.0f ? position : (position  + (Vector2)data.predatorPosition)/2);
        }

        private void SpawnOnStart()
        {
            if (entitiesCountOnStart <= 0) return;

            for (int i = 0; i < entitiesCountOnStart; i++)
            {
                SpawnEntity(Random.insideUnitCircle * spawnRadius, GetAnchor());
            }
        }

        private void SpawnOnMouse()
        {
            SpawnEntity(mainCamera.ScreenToWorldPoint(Input.mousePosition), GetAnchor());
        }

        public void SpawnEntity(Vector2 _position, Transform _anchor)
        {
            GameObject entity = Instantiate(entityPrefab, _position, Quaternion.identity, entitiesParent);
            data.entities.Add(entity.GetComponent<EntityBehaviour>().data);
            entity.GetComponent<EntityBehaviour>().SetAnchor(_anchor);
        }

        private Transform GetAnchor()
        {
            if (anchors.Length == 0) return transform;

            int index = anchorIndex;
            anchorIndex++;
            if (anchorIndex == anchors.Length) anchorIndex = 0;

            return anchors[index];
        }
        private void SetNeighbours(EntityData _entity)
        {
            _entity.neighbours = new List<EntityData>();

            //je ccrée une liste temporaire pour stocker de manière ordonnée par ordre croissant de distance les autres entité par rapport à l'entité testée.
            List<EntityData> tmpList = new List<EntityData>();

            //Je boucle dans la liste de la totalité des entités pour identifier les voisines de l'entité testée.
            for (int i = 0; i < data.entities.Count; i++)
            {
                //s'il s'agit de l'entité testée, je continue.
                if (data.entities[i] == _entity || _entity.IsInFOV(data.entities[i].position, settings.deadSpot)) continue; 

                //Si ma liste temporaire ordonnée est vide, je la remplis d'un premier élément servant de base de référence.
                if (tmpList.Count == 0) tmpList.Add(data.entities[i]);

                //Sinon, c'est que j'ai au moins un élément, et que je dois évaluer sa position dans la liste.
                else
                {
                    //float sqrdDistance = Mathf.Pow(entities[i].position.x - _entity.position.x, 2) + Mathf.Pow(entities[i].position.y - _entity.position.y, 2);

                    //Je stocke en mémoire la distance entre l'entité testée, et l'entité voisine évaluée.
                    float distance = Vector3.Distance(data.entities[i].position, _entity.position);

                    //Je décalre une variable m'indiquant si l'entité voisine évaluée a bien été ajoutée à la liste temporaire ordonnée.
                    bool added = false;

                    //Je boucle à l'intérieur de la liste temporaire ordonnée pour trouver où placer l'entité voisine évaluée.
                    for (int j = 0; j < tmpList.Count; j++)
                    {
                        //if (sqrdDistance < Mathf.Pow(entities[i].position.x - tmpList[j].position.x, 2) + Mathf.Pow(entities[i].position.y - tmpList[j].position.y, 2))

                        //je teste si la distance entre l'entité testée et l'entité voisine évaluée est inférieure à la distance entre l'entité voisine évaluée et l'entité à l'index j de la liste temporaire ordonnée
                        if (distance < Vector3.Distance(_entity.position, tmpList[j].position))
                        {
                            tmpList.Insert(j, data.entities[i]);
                            added = true;

                            break;
                        }
                    }

                    if (!added) tmpList.Add(data.entities[i]);
                }
            }

            if (tmpList.Count < settings.maxNeighbours) return;

            for (int i = 0; i < settings.maxNeighbours; i++)
            {
                if (Vector3.Distance(tmpList[i].position, _entity.position) < settings.inRange) _entity.neighbours.Add(tmpList[i]);
            }

            while (_entity.neighbours.Count < settings.minNeighbours)
            {
                _entity.neighbours.Add(tmpList[_entity.neighbours.Count]);
            }

            #region OLD CODE
            //foreach (EntityData testedEntity in entities)
            //{
            //    if (testedEntity == _entity) continue;


            //    if (Vector3.Distance(testedEntity.position, _entity.position) < settings.inRange) _entity.neighbours.Add(testedEntity);
            //    //else if (Vector3.Distance(testedEntity.projectedHorizontalPos, _entity.position) < settings.inRange) _entity.neighbours.Add(testedEntity);
            //    //else if (Vector3.Distance(testedEntity.projectedVerticalPos, _entity.position) < settings.inRange) _entity.neighbours.Add(testedEntity);
            //    //else if (Vector3.Distance(testedEntity.projectedPos, _entity.position) < settings.inRange) _entity.neighbours.Add(testedEntity);
            //}

            //if(_entity.neighbours.Count > 7)
            //{
            //    _entity.neighbours = _entity.neighbours.OrderBy(entity => Vector3.Distance(entity.position, _entity.position))
            //}
            #endregion
        }

        private void ManageEntities()
        {
            foreach (EntityData entity in data.entities)
            {
                if (Random.value < 0.75f) continue;

                SetNeighbours(entity);
            }
        }
    }
}