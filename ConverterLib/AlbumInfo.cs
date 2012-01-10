using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mp3Classify
{
    class AlbumInfo
    {
      
        string name;
        /// <summary>
        /// 专辑名称
        /// </summary>
        public string Name
        {
            get { return name; }
           // set { albumName = value; }
        }

      //  List<> songs;
        /// <summary>
        /// 专辑歌曲列表
        /// </summary>
        //internal List<ISongInfo> Songs
       // {
       //     get { return songs; }
            
       // }
        //public AlbumInfo(string name)
        //{
        //    this.name = name;
        //    songs = new List<ISongInfo>();
        //}
        ///// <summary>
        ///// 向专辑中添加歌曲
        ///// </summary>
        ///// <param name="song"></param>
        //internal void  AddSong(ISongInfo song)
        //{
        //    songs.Add(song);
        //}
        ///// <summary>
        ///// 从专辑中删除歌曲
        ///// </summary>
        ///// <param name="song"></param>
        ///// <returns></returns>
        //internal bool DeleteSong(ISongInfo song)
        //{
        //    return songs.Remove(song);
        //}



    }
}
