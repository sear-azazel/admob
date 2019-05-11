using UnityEngine;

namespace Azuma.Utils
{
    /// <summary>
    /// SingletonMonoBehaviour
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        /// <summary>
        /// インスタンス
        /// </summary>
        private static T instance;

        /// <summary>
        /// インスタンス取得
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));

                    if (instance == null)
                    {
                        Debug.LogWarning(typeof(T) + "is nothing");
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Awake
        /// </summary>
        protected virtual void Awake() => this.CheckInstance();

        /// <summary>
        /// インスタンスチェック
        /// </summary>
        /// <returns></returns>
        protected bool CheckInstance()
        {
            if (instance == null)
            {
                instance = (T)this;
                return true;
            }
            else if (Instance == this)
            {
                return true;
            }

            Destroy(this);

            return false;
        }
    }
}
