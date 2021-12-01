﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterCardGame.DeckManage;

namespace MonsterCardGame.Users
{
    class User
    {
        public string Name { get; private set; }
        //string NickName { get; set; }
        public AbstractDeckManager myDeck;  //change to private => add getter
        
        public User(string name)    //create user with random deck
        {
            this.Name = name;
            this.myDeck = new Deck();
            Console.WriteLine("Created User {0}", this.Name);
        }
    }
}
