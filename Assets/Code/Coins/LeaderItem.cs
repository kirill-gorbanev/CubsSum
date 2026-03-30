using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;

namespace Code
{
    public class LeaderItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text textName;
        [SerializeField] private TMP_Text textScore;
        [SerializeField] private Image imagePhoto;

        public async void Load(string names, int score, string photoUrl)
        {
            if (textName != null)
                textName.text = names;

            if (textScore != null)
                textScore.text = score.ToString();


            if (!string.IsNullOrEmpty(photoUrl) && imagePhoto != null)
            {
                await EX.LoadImageFromUrlAsync(photoUrl, imagePhoto);
            }
        }
    }

    public static class EX
    {
        public static async Task LoadImageFromUrlAsync(string url, Image imagePhoto)
        {
            try
            {
                using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
                {
                    var asyncOperation = request.SendWebRequest();


                    while (!asyncOperation.isDone)
                    {
                        await Task.Yield();
                    }

                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

                        if (texture != null && imagePhoto != null)
                        {
                            Sprite sprite = Sprite.Create(
                                texture,
                                new Rect(0, 0, texture.width, texture.height),
                                new Vector2(0.5f, 0.5f)
                            );

                            imagePhoto.sprite = sprite;
                        }
                    }
                    else
                    {
                        Debug.LogError($"Не удалось загрузить изображение: {request.error}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка при загрузке изображения: {ex.Message}");
            }
        }
    }
}