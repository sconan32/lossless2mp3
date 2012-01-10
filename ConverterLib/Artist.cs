using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mp3Classify
{
    class Artist
    {
        string name;

        public string Name
        {
            get { return name; }
            //set { name = value; }
        }
        List<AlbumInfo> albums;

        internal List<AlbumInfo> Albums
        {
            get { return albums; }
           // set { albums = value; }
        }
        public Artist(string name)
        {
            this.name = name;
            albums = new List<AlbumInfo>();
         //   albums.Add(new AlbumInfo("default"));
        }
        public override bool Equals(object obj)
        {
            return (this.name == ((Artist)obj).name);
        }
        public override string ToString()
        {
            return this.name ;
        }
    }
}
