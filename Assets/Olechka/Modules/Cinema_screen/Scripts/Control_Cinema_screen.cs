using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Olechka
{
    public class Control_Cinema_screen : MonoBehaviour
    {
        [Tooltip("Видео плеер")]
        [SerializeField]
        VideoPlayer VPlayer = null;

        [SerializeField]
        bool Start_bool = false;

        [SerializeField]
        bool Auto_next_bool = false;

        [SerializeField]
        string[] Url_video_array = { "https://dl.dropboxusercontent.com/scl/fi/j93ktm5776fmpbrlxktnk/Transcendental-Cha-Cha-Cha-Official-Music-Video.mp4?rlkey=w40kds1asrxhjhi2hd4rdlx6x&st=kudgx481&dl=1",
            "https://dl.dropboxusercontent.com/scl/fi/5alqjsh9b81l2yvxw9850/Aaron-Smith-Dancin.mp4?rlkey=rmliqf6dfspv878fuwckes038&st=ej04xlph&dl=1",
            "https://dl.dropboxusercontent.com/s/mbbk9suvuwbw96w/BEAST.mp4?dl=1",
            "https://dl.dropboxusercontent.com/s/szttz5qmdz3zzpx/%D0%9C%D0%BE%D1%82%D0%B8%D0%B2%D0%B0%D1%86%D0%B8%D1%8F%20-%20%D0%9C%D0%B0%D0%B9%D0%BD%D0%BA%D1%80%D0%B0%D1%84%D1%82%20%D0%9A%D0%BB%D0%B8%D0%BF.mp4?dl=1",
            "https://dl.dropboxusercontent.com/s/qitskpzcz4nc3ac/Mystery%20Skulls%20Animated%20-%20Ghost.mp4?dl=1",
            "https://dl.dropboxusercontent.com/s/4jqyc5xxuuyhfht/MiatriSs%20-%20Deadline.mp4?dl=1",
            "https://dl.dropboxusercontent.com/scl/fi/e577x4ws3h0crvyy8fx8k/ENHYPEN-_-Brought-The-Heat-Back-_-1theKILLPO-_-_-_-Performance-_-4K.mp4?rlkey=gmo474zfxfmp7orxqdsrpbklg&st=bnghv9uq&dl=1"
        };

        int Id_video = 0;

        bool Play_bool = false;


        [Foldout("Ивенты")]
        [Tooltip("Ивент при запуске")]
        [SerializeField]
        UnityEvent Play_event = new UnityEvent();

        [Foldout("Ивенты")]
        [Tooltip("Ивент при паузе")]
        [SerializeField]
        UnityEvent Pause_event = new UnityEvent();

        private void Start()
        {

            if (Start_bool) 
                Start_play(Url_video_array[0]);

            if (Auto_next_bool)
                VPlayer.loopPointReached += Next_play;
        }

        public void Play_and_pause()
        {
            if (!Play_bool)
                Continue();
            else
                Pause();
        }

        public void Start_play()
        {
            Start_play(Url_video_array[0]);
        }

        public void Start_play(int _id_url)
        {
            Start_play(Url_video_array[_id_url]);
        }


        /// <summary>
        /// Запустить трансляцию видео
        /// </summary>
        /// <param name="_url">URL адрес видео</param>
        public void Start_play(string _url)
        {
            VPlayer.url = _url;
            VPlayer.Play();
            Play_bool = true;
            Play_event.Invoke();
        }

        public void Pause()
        {
            VPlayer.Pause();

            Play_bool = false;

            Pause_event.Invoke();
        }

        public void Continue()
        {
            if(VPlayer.url == "")
                VPlayer.url = Url_video_array[0];

            VPlayer.Play();

            Play_bool = true;

            Play_event.Invoke();
        }

        public void Last_play()
        {
            Id_video--;

            if (Id_video < 0)
            {
                Id_video = Url_video_array.Length - 1;
            }

            Start_play(Id_video);
        }

        public void Last_play(VideoPlayer vp)
        {
            Last_play();
        }

        public void Next_play()
        {
            Id_video++;

            if (Id_video > Url_video_array.Length - 1)
            {
                Id_video = 0;
            }

            Start_play(Id_video);
        }

        public void Next_play(VideoPlayer vp)
        {
            Next_play();
        }
    }
}