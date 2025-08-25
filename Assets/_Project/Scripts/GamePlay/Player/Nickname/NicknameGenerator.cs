namespace _Project.Scripts.GamePlay.Player.Nickname
{
    public static class NicknameGenerator
    {
        static readonly string[] RandomNicknames = {
            "Player", "Warrior", "Hunter", "Mage", "Archer", "Knight", 
            "Rogue", "Paladin", "Wizard", "Berserker", "Assassin", "Monk",
            "Ranger", "Bard", "Cleric", "Druid", "Sorcerer", "Warlock"
        };

        public static string Generate()
        {
            string randomName = RandomNicknames[UnityEngine.Random.Range(0, RandomNicknames.Length)];
            return $"{randomName}{UnityEngine.Random.Range(100, 999)}";
        }
    }
}