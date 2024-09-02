using ConsoleApp1.Domain;
using ConsoleApp1.Services;




var tournament = new Tournament();

var groupList = DataLoader.LoadGroups();

var teams = DataLoader.CreateTeamMap(groupList);

ExibitionLoader.LoadExhibitions("../Data/exibitions.json", teams);


Console.WriteLine("-------------------------------------------------");



tournament.SimulateGroupStage(groupList);

tournament.PrintGroupStandings(groupList);



Console.WriteLine("-------------------------------------------------");


tournament.PrintFinalStandings(groupList);


Console.WriteLine("-------------------------------------------------");



var (potD, potE, potF, potG) = tournament.CreatePots(groupList);

tournament.PrintPots(potD, potE, potF, potG);



Console.WriteLine("-------------------------------------------------");



var quarterFinalPairs = tournament.FormQuarterfinalPairs(potD, potE, potF, potG);

tournament.PrintEliminationPhase(quarterFinalPairs);

var quareterFinalWinners = tournament.SimulateKnockoutStage("Četvrtfinale", quarterFinalPairs );




Console.WriteLine("-------------------------------------------------");



var semiFinalPairs = tournament.FormSemifinalPairs(quareterFinalWinners);
var semiFinalWinners = tournament.SimulateKnockoutStage("Polufinale", semiFinalPairs);



Console.WriteLine("-------------------------------------------------");

tournament.SimulateBronzeAndGoldMatches(semiFinalWinners);