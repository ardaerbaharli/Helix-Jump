using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    public class ArdaTween : MonoBehaviour
    {
        public static Hashtable Hash(params object[] args)
        {
            var hashTable = new Hashtable(args.Length / 2);
            if (args.Length % 2 != 0)
            {
                Debug.LogError("Tween Error: Hash requires an even number of arguments!");
                return null;
            }

            var i = 0;
            while (i < args.Length - 1)
            {
                hashTable.Add(args[i], args[i + 1]);
                i += 2;
            }

            return hashTable;
        }

        private static void Launch(GameObject actor, string methodName, Hashtable args)
        {
            var t = actor.AddComponent<ArdaTween>();
            args.Add("actor", actor);
            t.StartCoroutine($"{methodName}Coroutine", args);
        }

        private void FinishProgram(Hashtable args)
        {
            var actor = args["actor"] as GameObject;
            if (actor == null) return;


            if (args.Contains("setActive"))
                actor.SetActive((bool) args["setActive"]);

            if (args.Contains("destroy") && (bool) args["destroy"])
                Destroy(actor);

            if (args.Contains("onComplete"))
                actor.SendMessage((string) args["onComplete"], SendMessageOptions.DontRequireReceiver);

            Destroy(actor.GetComponent<ArdaTween>());
        }

        public static void RotateTo(GameObject actor, Hashtable args)
        {
            args.Add("Transform", actor.transform);
            Launch(actor, System.Reflection.MethodBase.GetCurrentMethod().Name, args);
        }


        public static void MoveTo(GameObject actor, Hashtable args)
        {
            args.Add("transform", actor.transform);
            Launch(actor, System.Reflection.MethodBase.GetCurrentMethod().Name, args);
        }

        public static void ImageColorTo(GameObject actor, Hashtable args)
        {
            Launch(actor, System.Reflection.MethodBase.GetCurrentMethod().Name, args);
        }


        private IEnumerator RotateToCoroutine(Hashtable args)
        {
            var t = (Transform) args["transform"];
            var targetRotation = (Vector3) args["targetRotation"];
            var time = (float) args["duration"];

            var g = t.gameObject;
            var startRotation = t.rotation.eulerAngles;
            var deltaTime = 0f;
            while (deltaTime < 1)
            {
                if (!CheckObject(g))
                    break;
                deltaTime += Time.deltaTime / time;
                t.rotation = Quaternion.Euler(Vector3.Lerp(startRotation, targetRotation, deltaTime));
                yield return null;
            }

            FinishProgram(args);
        }

        private IEnumerator MoveToCoroutine(Hashtable args)
        {
            RectTransform rect = null;
            var isRect = false;
            if (args.Contains("rectTransform"))
            {
                rect = (RectTransform) args["rectTransform"];
                isRect = true;
            }

            var t = (Transform) args["transform"];
            var position = (Vector3) args["targetPosition"];
            var duration = (float) args["duration"];
            var g = t.gameObject;

            var isLocalPosition = false;
            if (args.Contains("isLocalPosition"))
                isLocalPosition = (bool) args["isLocalPosition"];

            if (isRect)
            {
                var startPosition = rect.anchoredPosition;
                var t1 = 0f;
                while (t1 < duration)
                {
                    if (!CheckObject(g))
                        break;
                    t1 += Time.deltaTime;
                    rect.anchoredPosition = Vector3.Lerp(startPosition, position, t1 / duration);
                    yield return null;
                }
            }
            else
            {
                if (isLocalPosition)
                {
                    var startPosition = t.localPosition;
                    var t1 = 0f;
                    while (t1 < duration)
                    {
                        if (!CheckObject(g))
                            break;
                        t1 += Time.deltaTime;
                        t.localPosition = Vector3.Lerp(startPosition, position, t1 / duration);
                        yield return null;
                    }
                }
                else
                {
                    var startPosition = t.position;
                    var t1 = 0f;
                    while (t1 < duration)
                    {
                        if (!CheckObject(g))
                            break;
                        t1 += Time.deltaTime;
                        t.position = Vector3.Lerp(startPosition, position, t1 / duration);
                        yield return null;
                    }
                }
            }

            FinishProgram(args);
        }

        private bool CheckObject(GameObject g)
        {
            return g != null;
        }


        private IEnumerator ImageColorToCoroutine(Hashtable args)
        {
            var image = (Image) args["image"];
            var color = (Color) args["color"];
            var duration = (float) args["duration"];
            var g = image.gameObject;
            var startColor = image.color;
            var t = 0f;
            while (t < duration)
            {
                if (!CheckObject(g))
                    break;
                t += Time.deltaTime;
                image.color = Color.Lerp(startColor, color, t / duration);
                yield return null;
            }

            FinishProgram(args);
        }
    }
}