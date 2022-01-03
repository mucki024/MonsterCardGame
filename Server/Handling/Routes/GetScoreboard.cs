﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MonsterCardGame.Database;
using Newtonsoft.Json.Linq;

namespace MonsterCardGame.Server
{
    public class GetScoreboard :Handler
    {
        private Dictionary<int, string> _user = new Dictionary<int, string>();
        public GetScoreboard( AuthLevel level) :base(level)
        {
            
        }

        public override void DeserializeMessage(string message)
        {

        }

        public override void Handle(Response res,string token)
        {
            if (!CheckAuth(res, token))
                return;

            string username;
            if (!Session.SessionDic.TryGetValue(token, out username))
            {
                //key is not in dic => should not happen cause of checkauth
                Console.WriteLine("Key not in Dictionary");
                return;
            }
          
            IScoreDao scoredao = new ScoreDao();
            List<ScoreModel> scoreModel =scoredao.ShowScoreBoard();
            getUserList();
            JArray array = new JArray();
            foreach (var singleScore in scoreModel)
            {
                JObject obj = new JObject();
                if (!_user.ContainsKey(singleScore.UID))
                {
                    Console.WriteLine("couldnt get username from dictionary for scoreboard");
                    continue;
                }
                obj["username"] = _user[singleScore.UID];
                obj["elo"] = singleScore.Elo;
                obj["wins"] = singleScore.Wins;
                obj["games"] = singleScore.Games;
                obj["loses"] = singleScore.Loses;
                array.Add(obj);
            }

            res.SendResponse(responseType.OK, JsonConvert.SerializeObject(array));
            Console.WriteLine("Sent scoreboard");
        }

        private void getUserList()
        {
            UserDao userDao = new UserDao();
            List<UserModel> modelList = userDao.GetAllUsers();
            modelList.ForEach((item) => _user.Add(item.UID, item.Username));
        }
    }
}