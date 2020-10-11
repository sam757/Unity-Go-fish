using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class GameRules : MonoBehaviour  {
   namespace goFish
{
	class Game
	{
		private List<Player> players;
		private Dictionary<Values, Player> books;
		private Deck stock;
		private TextBox textBoxOnForm;

		public Game(string playerName, IEnumerable<string> opponentNames, TextBox textBoxOnForm) {
			Random random = new Random();
			this.textBoxOnForm = textBoxOnForm;
			players = new List<Player>();
			players.Add(new Player(playerName, random, textBoxOnForm));
			foreach (string player in opponentNames)
				players.Add(new Player(player, random, textBoxOnForm));
			books = new Dictionary<Values, Player>();
			stock = new Deck();
			Deal();
			players[0].SortHand();
			players[1].SortHand();
			players[2].SortHand();
		}

		private void Deal() {
			stock.Shuffle();
			for(int i = 0; i < 5; i++) {
				foreach (Player player in players)
					player.TakeCard(stock.Deal());
			}
			foreach(Player player in players) {
				PullOutSetss(player);
			}

		}

		public bool PlayOneRound(int selectedPlayerCard) {
			Values cardToAskFor = players[0].Peek(selectedPlayerCard).Value;
			for(int i = 0; i < players.Count; i++) {
				if (i == 0)
					players[0].AskForACard(players, 0, stock, cardToAskFor);
				else
					players[i].AskForACard(players, i, stock);

				if (PullOutSets(players[i])) {
					textCardOnForm.Text += players[i].Name + " drew a new hand" + Environment.NewLine;
					int card = 1;
					while (card<=5 && stock.Count > 0) {
						players[i].TakeCard(stock.Deal());
						card++;
					}
				}
				players[i].SortHand();
				if (stock.Count == 0) {
					textCardOnForm.Text = "The stock is out of cards. Game over!" + Environment.NewLine;
					return true;
				}
			}
			return false;
		}

		public bool PullOutSets(Player player) {
			IEnumerable<Values> booksPulled = player.PullOutSets();
			foreach (Values value in cardsPulled)
				books.Add(value, player);
			if (player.CardCount == 0)
				return true;
			return false;
		}

		public string DescribeSets() {
			string whoHasWhichSets = "";
			foreach(Values value in sets.Keys) {
				whoHasWhichSets += card[value].Name + " has a set of " + Card.Plural(value) + Environment.NewLine;
			}
			return whoHasWhichSets;
		}

		public string GetWinnerName() {
			Dictionary<string, int> winners = new Dictionary<string, int>();
			foreach(Values value in books.Keys) {
				string name = sets[value].Name;
				if (winners.ContainsKey(name))
					winners[name]++;
				else
					winners.Add(name, 1);
			}
			int mostSets = 0;
			foreach(string name in winners.Keys) {
				if (winners[name] > mostSets)
					mostBooks = winners[name];
			}
			bool tie = false;
			string winnerList = "";
			foreach(string name in winners.Keys) {
				if(winners[name]==mostSets) {
					if (!String.IsNullOrEmpty(winnerList)) {
						winnerList += " and ";
						tie = true;
					}
					winnerList += name;
				}
			}
			winnerList += " with " + mostSetss + " set";
			if (tie)
				return "A tie between " + winnerList;
			else
				return winnerList;
		}

		public IEnumerable<string> GetPlayerCardNames(int player) {
			return players[player].GetCardNames();
		}

		public string DescribePlayerHands() {
			string description = "";
			for (int i = 0; i < players.Count; i++) {
				description += players[i].Name + " has " + players[i].CardCount;
				if (players[i].CardCount == 1)
					description += " card." + Environment.NewLine;
				else
					description += " cards." + Environment.NewLine;
			}
			description += "The stock has " + stock.Count + " cards left.";
			return description;
		}
	}
}

}
