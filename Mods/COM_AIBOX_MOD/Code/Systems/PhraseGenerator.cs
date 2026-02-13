using System;
using System.Collections.Generic;
using System.Linq;

namespace AIBox
{
    public static class PhraseGenerator
    {
        private static System.Random rnd = new System.Random();
        
        public static string Generate(ChatterContext ctx)
        {
            if (ctx == null) return GetGenericPhrase();
            string category = SelectCategory(ctx);
            string phrase = BuildPhrase(category, ctx);
            return ApplyVariation(phrase);
        }
        
        private static string SelectCategory(ChatterContext ctx)
        {
            List<(string cat, int weight)> options = new List<(string, int)>();
            
            if (ctx.AtWar) options.Add(("war", 40));
            if (!string.IsNullOrEmpty(ctx.LastAIAction)) options.Add(("ai_decision", 30));
            if (ctx.HighTaxes) options.Add(("taxes_high", 25));
            if (ctx.LowTaxes) options.Add(("taxes_low", 10));
            if (ctx.InDebt) options.Add(("debt", 20));
            if (ctx.Prosperous) options.Add(("prosperous", 15));
            if (ctx.IsHungry) options.Add(("hungry", 25));
            if (ctx.IsInjured) options.Add(("injured", 20));
            if (ctx.Mood == MoodLevel.Miserable) options.Add(("miserable", 20));
            if (ctx.Mood == MoodLevel.Ecstatic) options.Add(("happy", 20));
            if (ctx.GlobalWar) options.Add(("global_war", 15));
            if (ctx.Traits.Contains("greedy")) options.Add(("greedy", 15));
            if (ctx.Traits.Contains("wise")) options.Add(("wise", 15));
            if (ctx.Traits.Contains("aggressive")) options.Add(("aggressive", 15));
            if (ctx.Role == "Soldier") options.Add(("soldier", 20));
            if (ctx.Role == "King") options.Add(("king", 25));
            
            options.Add(("generic", 10));
            options.Add(("work", 10));
            options.Add(("gossip", 10));
            options.Add(("weather", 8));
            options.Add(("philosophy", 5));
            options.Add(("daily", 10));
            
            int totalWeight = options.Sum(o => o.weight);
            int roll = rnd.Next(totalWeight);
            int cumulative = 0;
            foreach (var opt in options) { cumulative += opt.weight; if (roll < cumulative) return opt.cat; }
            return "generic";
        }
        
        private static string BuildPhrase(string category, ChatterContext ctx)
        {
            switch (category)
            {
                case "war": return BuildWarPhrase(ctx);
                case "ai_decision": return BuildAIDecisionPhrase(ctx);
                case "taxes_high": return BuildHighTaxPhrase(ctx);
                case "taxes_low": return BuildLowTaxPhrase(ctx);
                case "debt": return BuildDebtPhrase(ctx);
                case "prosperous": return BuildProsperousPhrase(ctx);
                case "hungry": return BuildHungryPhrase(ctx);
                case "injured": return BuildInjuredPhrase(ctx);
                case "miserable": return BuildMiserablePhrase(ctx);
                case "happy": return BuildHappyPhrase(ctx);
                case "global_war": return BuildGlobalWarPhrase(ctx);
                case "work": return BuildWorkPhrase(ctx);
                case "gossip": return BuildGossipPhrase(ctx);
                case "weather": return BuildWeatherPhrase(ctx);
                case "philosophy": return BuildPhilosophyPhrase(ctx);
                case "daily": return BuildDailyPhrase(ctx);
                case "greedy": return BuildGreedyPhrase(ctx);
                case "wise": return BuildWisePhrase(ctx);
                case "aggressive": return BuildAggressivePhrase(ctx);
                case "soldier": return BuildSoldierPhrase(ctx);
                case "king": return BuildKingPhrase(ctx);
                default: return GetGenericPhrase();
            }
        }
        
        // ============ WAR PHRASES (25 templates) ============
        private static string BuildWarPhrase(ChatterContext ctx)
        {
            string enemy = ctx.EnemyName ?? "the enemy";
            string[] templates = {
                $"{P("By the gods,", "I heard", "They say", "Word is")} {enemy} {P("threatens us", "marches on our borders", "gathers their forces", "prepares for invasion")}{P("!", "...", ". I fear the worst.", ". May the gods help us.")}",
                $"War with {enemy}{P("!", "...")} {P("May the gods protect us.", "We must be strong.", "When will it end?", "I pray for our soldiers.")}",
                $"{P("My", "Our")} {P("son", "brother", "family", "loved ones")} {P("fights", "marches", "bleeds")} against {enemy}. {P("I pray for their return.", "Gods be with them.", "This war must end.", "Bring them home safe.")}",
                $"{enemy}... {P("those savages!", "when will they fall?", "I curse their name!", "may they burn!", "filthy invaders!")}",
                $"The {P("King", "Crown", "Ruler", "Liege")} leads us against {enemy}. {P("For glory!", "May we prevail.", "I hope it's worth it.", "Victory or death!")}",
                $"I saw {P("soldiers", "warriors", "troops")} marching toward {enemy} lands. {P("Grim faces.", "They looked ready.", "Some were just boys.", "Gods protect them.")}",
                $"The {P("drums", "horns", "bells")} of war echo through the land. {P("My heart trembles.", "We are ready.", "The end is near.", "Steel yourselves.")}",
                $"They say {enemy} has {P("ten thousand", "countless", "endless", "mighty")} warriors. {P("We're doomed.", "But we have heart!", "Numbers mean nothing.", "I don't believe it.")}",
                $"Blood will flow between us and {enemy}. {P("Such waste.", "For what?", "Honor demands it.", "There's no other way.")}",
                $"I lost {P("a friend", "my cousin", "someone dear")} to {enemy}'s blades. {P("I'll never forget.", "Vengeance will come.", "War takes everything.", "They will pay.")}",
                $"The {P("nights", "days")} grow darker with this war against {enemy}. {P("When will dawn come?", "I've forgotten peace.", "My children cry.", "Home feels empty.")}",
                $"Refugees from the {enemy} border... {P("their eyes are hollow.", "they've lost everything.", "it could be us next.", "I shared my bread.")}",
                $"Every {P("night", "morning", "moment")}, I fear {enemy} will reach our {P("gates", "homes", "village")}. {P("Sleep eludes me.", "I keep a blade near.", "The children sense it too.", "Pray for walls.")}",
                $"Our {P("brave", "valiant", "noble")} soldiers stand against {enemy}. {P("Pride fills my heart.", "May they return.", "The realm depends on them.", "Send them our strength.")}",
                $"War with {enemy} was {P("inevitable", "foolish", "necessary", "avoidable")}. {P("Now we pay the price.", "History repeats.", "The Crown knows best.", "Or so they say.")}",
                $"I {P("heard", "saw")} {enemy} prisoners being marched through town. {P("They looked broken.", "Even enemies suffer.", "Good riddance.", "War makes monsters of us all.")}",
                $"The {P("fields", "farms", "roads")} lie abandoned because of {enemy}. {P("Who will harvest?", "Famine follows war.", "The land weeps.", "Curse this conflict.")}",
                $"My {P("axe", "sword", "blade")} is ready for {enemy}! {P("Let them come!", "I'll defend my home!", "For the kingdom!", "No mercy!")}",
                $"Peace with {enemy}? {P("Never!", "If only...", "I'd forgive anything for it.", "A dream now.")}",
                $"The {P("healers", "priests", "nurses")} work day and night for the war wounded. {P("Bless them.", "So much suffering.", "They're the true heroes.", "It never ends.")}",
                $"Widows weep for those lost to {enemy}. {P("Too many.", "The cost of glory.", "I dare not count.", "May it end soon.")}",
                $"They conscripted the {P("baker's son", "smith's apprentice", "young farmer")} to fight {enemy}. {P("He's just a boy.", "We're running out of men.", "War spares no one.", "Gods guide his spear.")}",
                $"Victory against {enemy} will be {P("sweet", "hollow", "hard-won", "bloody")}. {P("If it comes.", "At what cost?", "I'll toast to it.", "If we survive.")}",
                $"Every {P("coin", "prayer", "thought")} goes to the war against {enemy}. {P("What's left for us?", "It's worth it.", "The burden is heavy.", "We sacrifice together.")}",
                $"I {P("dream", "think", "worry")} about {enemy} every night. {P("Sleep brings no peace.", "When will this end?", "My mind won't rest.", "Fear is my companion now.")}"
            };
            return templates[rnd.Next(templates.Length)];
        }
        
        // ============ AI DECISION PHRASES ============
        private static string BuildAIDecisionPhrase(ChatterContext ctx)
        {
            string action = ctx.LastAIAction?.ToLower() ?? "something";
            string target = ctx.LastAITarget ?? "them";
            
            // === DIPLOMACY ===
            if (action.Contains("war") || action.Contains("attack"))
            {
                string[] t = {
                    $"Did you hear? The {P("King", "Crown", "Ruler")} declared war on {target}!",
                    $"War with {target}! {P("Madness!", "Bold move.", "We're doomed.", "About time!", "The gods help us.")}",
                    $"They say we march against {target} soon... {P("prepare yourselves.", "may we prevail.", "I'm not ready.")}",
                    $"The {P("King", "Crown")} has spoken: {target} shall {P("fall", "burn", "submit", "know our wrath")}!",
                    $"So it begins... war with {target}. {P("I feared this day.", "Finally!", "What drove them to this?")}",
                    $"The palace announced war on {target}. The {P("taverns fell silent", "streets emptied", "children cried")}.",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("peace") || action.Contains("treaty"))
            {
                string[] t = {
                    $"Peace with {target}! {P("Finally!", "Thank the gods.", "About time.", "My son can come home!")}",
                    $"The war is over. {P("We can breathe.", "At what cost?", "Praise be!", "Now we rebuild.")}",
                    $"They signed a treaty with {target}. {P("Hope blooms.", "Trust is fragile.", "Peace at last!")}",
                    $"No more fighting with {target}? {P("I'll believe it when I see it.", "Blessed news!", "Time heals all.")}",
                    $"The {P("King", "Crown")} made peace with {target}. {P("Wisdom prevails!", "A new dawn.", "My prayers answered.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("alliance"))
            {
                string[] t = {
                    $"Alliance with {target}! {P("Strong together.", "New friends.", "United we stand.", "A powerful pact.")}",
                    $"We ally with {target} now. {P("A wise choice?", "Time will tell.", "Stronger together!", "I trust the Crown.")}",
                    $"The {P("King", "Crown")} joined hands with {target}. {P("Allies at last!", "May it bring peace.", "Smart diplomacy.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("threaten"))
            {
                string[] t = {
                    $"The {P("King", "Crown")} sent threats to {target}! {P("Bold!", "Dangerous game.", "They won't like that.", "War looms.")}",
                    $"We're threatening {target}? {P("Hope they back down.", "Risky move.", "Show of strength.", "Diplomacy of steel.")}",
                    $"Did you hear? The Crown threatened {target}. {P("Tension rises.", "War drums beat.", "Brave or foolish?")}",
                    $"Threatening {target}! {P("Saber rattling.", "The army stirs.", "Intimidation tactics.", "Power play.")}",
                    $"The {P("heralds", "messengers")} delivered an ultimatum to {target}. {P("Comply or else!", "Bold words.", "They won't back down.", "Tension mounts.")}",
                    $"An ultimatum to {target}! {P("Submit or face consequences.", "Strong stance.", "Gambling with war.", "The realm holds its breath.")}",
                    $"They say the {P("King", "Crown")} threatened to {P("invade", "destroy", "crush")} {target}. {P("Words or action?", "Posturing?", "I believe it.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("pact") || action.Contains("nonaggression"))
            {
                string[] t = {
                    $"A pact with {target}! {P("Peace assured?", "Trust is fragile.", "Better than war.", "Diplomacy works.")}",
                    $"Non-aggression pact signed. {P("We can relax.", "For now at least.", "Paper promises.", "Smart move.")}",
                    $"They say {target} and us won't fight. {P("Good news!", "I hope it holds.", "Finally some peace.")}",
                    $"The {P("King", "Crown")} signed a pact with {target}. {P("Mutual respect.", "Cautious friendship.", "Defense treaty?")}",
                    $"Non-aggression confirmed! {P("Sleep easier tonight.", "One less enemy.", "Treaties can break...", "But for now, peace.")}",
                    $"Peace treaty with {target}! {P("No more fear.", "The borders calm.", "Merchants celebrate.", "Trade resumes.")}",
                    $"A formal agreement with {target}. {P("Official peace.", "Binding words.", "May it last forever.", "Skeptics remain.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("message") || action.Contains("envoy"))
            {
                string[] t = {
                    $"The {P("King", "Crown")} sent a message to {target}. {P("Diplomacy in action.", "Words before swords.", "What did it say?")}",
                    $"Envoys heading to {target}! {P("Good sign.", "Hope they listen.", "Negotiation is wise.")}",
                    $"Diplomatic mission to {target}. {P("Talking is better than fighting.", "What terms offered?", "The council debates.")}",
                    $"Royal messengers dispatched to {target}. {P("Urgent news?", "Seeking terms.", "Communication opens doors.")}",
                    $"The {P("ambassadors", "diplomats")} travel to {target}. {P("Peace efforts?", "Trade discussions?", "Alliance talks?")}",
                    $"Word sent to {target}! {P("The Crown reaches out.", "Dialogue begins.", "Better than silence.", "May they respond well.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            
            // === ECONOMY ===
            else if (action.Contains("tax"))
            {
                string[] t = {
                    $"The {P("King", "Crown")} changed the taxes again. {P("Typical.", "Here we go...", "My coin purse weeps.")}",
                    $"New tax policy! {P("Will it help?", "We'll see.", "More burden on us.", "The Crown needs gold.")}",
                    $"Taxes are {P("going up", "changing", "being adjusted")}. {P("No surprise there.", "What else is new?")}",
                    $"Tax reform announced! {P("Reform they call it.", "Same old story.", "Different name, same pain.")}",
                    $"The {P("taxman cometh", "collectors prepare", "treasury demands")}. {P("Hide your coin!", "Brace yourselves.", "Nothing escapes them.")}",
                    $"Another tax {P("increase", "adjustment", "revision")}. {P("The Crown's appetite grows.", "We pay, always.", "Such is life.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("austerity"))
            {
                string[] t = {
                    $"Austerity measures announced! {P("Tighten your belts.", "Hard times ahead.", "The Crown cuts spending.")}",
                    $"They say we must {P("save", "sacrifice", "endure")}. {P("Austerity...", "Noble suffering.", "For how long?")}",
                    $"The Crown tightens the purse. {P("Less for all.", "Necessary evil?", "I fear the worst.")}",
                    $"Belt-tightening ordered! {P("The kingdom saves.", "Cuts everywhere.", "Lean times ahead.", "We'll manage.")}",
                    $"Austerity! {P("Fewer festivals.", "Less food at court.", "The army shrinks.", "Everyone suffers.")}",
                    $"Spending cuts across the realm. {P("Schools close.", "Roads decay.", "But the Crown survives.", "Priorities.")}",
                    $"The {P("treasury", "palace", "Crown")} demands {P("sacrifice", "restraint", "patience")}. {P("Easy for them to say.", "From their golden halls.", "We obey.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("stimulus"))
            {
                string[] t = {
                    $"Stimulus spending! {P("Gold flows!", "The Crown invests.", "Good for business!", "Jobs for all?")}",
                    $"The {P("King", "Crown")} is spending on the people! {P("Generous!", "Buying loyalty?", "I'll take it.")}",
                    $"Money flowing into the economy. {P("Finally!", "Smart move.", "Growth awaits.", "Markets rejoice.")}",
                    $"Economic stimulus announced! {P("Coin for everyone?", "Infrastructure projects.", "The economy breathes.", "Hope rises.")}",
                    $"The Crown opens the coffers! {P("Public works!", "Employment rises.", "Prosperity planned.", "History in making.")}",
                    $"Investment in the realm! {P("New roads.", "Better farms.", "Stronger defenses.", "A brighter future.")}",
                    $"Stimulus gold reaches the people. {P("At last!", "The Crown remembers us.", "Spend wisely.", "Don't waste it.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("protectionism") || action.Contains("tariff"))
            {
                string[] t = {
                    $"Trade barriers going up! {P("Protect our markets.", "Foreign goods cost more.", "Merchants grumble.")}",
                    $"Protectionist policies! {P("Local goods first.", "Prices will rise.", "Less variety.", "For our own good?")}",
                    $"Tariffs imposed! {P("Import taxes rise.", "Foreign trade slows.", "Our craftsmen rejoice.", "Others complain.")}",
                    $"The Crown protects local trade. {P("Buy domestic!", "Support our own.", "Quality varies.", "Prices stable.")}",
                    $"Trade restrictions announced. {P("Fewer foreign goods.", "Self-sufficiency?", "Markets adapt.", "Smugglers rejoice.")}",
                    $"Protectionism! {P("Walls around commerce.", "Our gold stays here.", "Innovation stifles?", "Debate rages.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("freemarket") || action.Contains("free market"))
            {
                string[] t = {
                    $"Free market declared! {P("Trade flows freely.", "Competition rises.", "Merchants celebrate.", "May the best win.")}",
                    $"No more restrictions! {P("Open borders for trade.", "Global goods flood in.", "Prices might drop.")}",
                    $"Free trade policy! {P("Markets liberated.", "Competition intensifies.", "Quality improves.", "Prices drop.")}",
                    $"The Crown embraces free trade. {P("Borders open.", "Merchants rejoice.", "New opportunities.", "Risk and reward.")}",
                    $"Trade barriers fall! {P("Goods flow freely.", "Foreign merchants arrive.", "Local shops worry.", "Adapt or perish.")}",
                    $"Open markets declared! {P("Capitalism triumphs.", "Innovation encouraged.", "The strong thrive.", "Others struggle.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            
            // === MONETARY ===
            else if (action.Contains("print") || action.Contains("money supply"))
            {
                string[] t = {
                    $"The Crown is {P("printing money", "minting coins", "expanding currency")}! {P("Inflation coming?", "More coin for all?", "Dangerous game.")}",
                    $"More {P("gold", "coins", "currency")} in circulation. {P("Prices will rise.", "Short-term relief.", "Long-term pain?")}",
                    $"They're devaluing our currency! {P("My savings shrink.", "Bread costs more.", "Economics confuse me.")}",
                    $"The {P("mints", "forges", "treasuries")} work overtime! {P("More coin floods markets.", "Inflation inevitable.", "Buy goods now!")}",
                    $"Currency expansion announced. {P("Sounds fancy.", "Means prices rise.", "Hold onto real goods.", "Paper promises.")}",
                    $"They're flooding the realm with {P("fresh coin", "new money", "minted gold")}. {P("Everyone feels richer.", "Until prices catch up.", "Short-sighted.")}",
                    $"Money printing! {P("The Crown's solution to everything.", "Debtors rejoice.", "Savers weep.", "History repeats.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("burn") || action.Contains("deflation"))
            {
                string[] t = {
                    $"The Crown is {P("burning money", "reducing currency", "tightening supply")}! {P("Deflation approaches.", "Coin becomes precious.")}",
                    $"Less money in circulation. {P("Prices should drop.", "Harder to get loans.", "Strange policy.")}",
                    $"Currency being {P("destroyed", "withdrawn", "burned")}. {P("Each coin worth more.", "Wages feel smaller.", "Debt heavier.")}",
                    $"Monetary tightening! {P("Coin becomes scarce.", "Prices should fall.", "Or will they?", "Uncertainty grows.")}",
                    $"The Crown reduces the money supply. {P("Deflation policy.", "Strengthen the currency.", "At what cost?")}",
                    $"They're making each coin more {P("valuable", "precious", "scarce")}. {P("Good for savers.", "Bad for borrowers.", "Complex economics.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("loan") || action.Contains("borrow"))
            {
                string[] t = {
                    $"The Crown took a loan! {P("From whom?", "Debt piles up.", "Quick gold, slow pain.", "We'll pay it back... right?")}",
                    $"Borrowing gold again. {P("To fund what?", "Interest grows.", "The bankers smile.", "Future generations weep.")}",
                    $"The realm goes deeper in debt. {P("Another loan signed.", "Creditors circle.", "When will it end?", "Mortgaging our future.")}",
                    $"A new loan from {P("foreign banks", "wealthy merchants", "other kingdoms")}. {P("Desperation?", "Investment?", "Both?", "Time will tell.")}",
                    $"The Crown borrows again. {P("Easy money today.", "Hard payments tomorrow.", "Someone profits.", "Not us.")}",
                    $"Debt grows! {P("The treasury borrows more.", "Interest compounds.", "Children will repay.", "Reckless or necessary?")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("repay") || action.Contains("debt"))
            {
                string[] t = {
                    $"The Crown repaid a debt! {P("Responsible!", "Finally!", "Less burden now.", "Creditors pleased.")}",
                    $"Debt is being paid off. {P("Good governance.", "Sacrifice pays off.", "Freedom approaches.")}",
                    $"Debt reduction announced! {P("The realm breathes easier.", "Credit restored.", "Fewer chains.", "Progress!")}",
                    $"The Crown honors its debts. {P("Trustworthy!", "Our word is gold.", "Creditors smile.", "Trade improves.")}",
                    $"Paying off the bankers. {P("At last!", "One burden less.", "How many remain?", "Step by step.")}",
                    $"Debt repayment! {P("The treasury empties.", "But bonds break.", "Freedom earned.", "Fiscal responsibility.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            
            // === COVERT ===
            else if (action.Contains("spy") || action.Contains("espionage"))
            {
                string[] t = {
                    $"Spies sent to {target}! {P("Secrets will flow.", "Risky business.", "Knowledge is power.", "Shh, don't tell anyone.")}",
                    $"The Crown has eyes in {target}. {P("Better than war.", "What will we learn?", "Sneaky, sneaky.")}",
                    $"Espionage against {target}! {P("I didn't hear anything.", "Clever move.", "Walls have ears.")}",
                    $"Shadow operatives infiltrate {target}. {P("Invisible war.", "Information gathered.", "They'll never know.", "Unless caught.")}",
                    $"The {P("spymasters", "intelligence agents", "scouts")} target {target}. {P("Secrets extracted.", "Weaknesses found.", "Leverage gained.")}",
                    $"Covert operations in {target}! {P("The silent war.", "Daggers in shadows.", "Trust no one.", "Effective.")}",
                    $"Eyes and ears in {target}. {P("We watch.", "We listen.", "We know.", "They suspect nothing.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("trainarmy") || action.Contains("train army") || action.Contains("military"))
            {
                string[] t = {
                    $"The army is training harder! {P("War coming?", "Strong defense.", "Soldiers everywhere.", "Drums in the barracks.")}",
                    $"Military buildup announced. {P("Preparing for what?", "Better safe than sorry.", "Young men enlisted.")}",
                    $"Troops drilling day and night! {P("Discipline increased.", "Ready for battle.", "Morale high.", "Or is it fear?")}",
                    $"The {P("barracks", "training grounds", "war camps")} overflow with recruits. {P("An army grows.", "Steel sharpened.", "Blood will flow.")}",
                    $"Military expansion! {P("More soldiers.", "Better weapons.", "Fortifications rise.", "Who do we fear?")}",
                    $"War preparations continue. {P("The Crown expects conflict.", "Train hard, fight easy.", "Every man a warrior.")}",
                    $"The army swells in size. {P("Conscription?", "Volunteers?", "Gold attracts fighters.", "Ready for anything.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("sabotage"))
            {
                string[] t = {
                    $"Sabotage against {target}! {P("Dirty tactics.", "Effective though.", "They'll never know.", "Dangerous game.")}",
                    $"The Crown ordered sabotage. {P("I heard nothing.", "Shadow operations.", "War by other means.")}",
                    $"Covert destruction in {target}! {P("Supply lines cut.", "Bridges burned.", "Chaos sown.", "Deniable.")}",
                    $"Saboteurs infiltrate {target}. {P("Poison wells.", "Burn stores.", "Weaken defenses.", "Silent war.")}",
                    $"Operations to destabilize {target}! {P("Accidents happen.", "Mills burn.", "Ships sink.", "No evidence.")}",
                    $"Shadow strikes against {target}. {P("Infrastructure crumbles.", "Blame the weather.", "Perfect execution.", "War without war.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("assassin"))
            {
                string[] t = {
                    $"An assassination was ordered! {P("Dark deeds.", "Blood on someone's hands.", "May the gods forgive.", "Whispers only.")}",
                    $"They say someone in {target} was... removed. {P("I don't want to know.", "Politics is deadly.", "Chilling news.")}",
                    $"A target in {target} eliminated. {P("Silent blade.", "No witnesses.", "Justice or murder?", "The Crown decides.")}",
                    $"Assassination! {P("Shadows claim another.", "A life for the realm.", "Necessary evil?", "I heard nothing.")}",
                    $"The {P("knife", "poison", "arrow")} found its mark in {target}. {P("Swift end.", "No trial.", "Royal decree.", "Shh...")}",
                    $"Someone of importance in {target}... gone. {P("Heart failure, they say.", "Convenient timing.", "The Crown's reach.", "Fear spreads.")}",
                    $"Dark orders fulfilled. {P("A death in {target}.", "No one will speak of it.", "Power's cold embrace.", "May they find peace.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            
            // === MARKET ===
            else if (action.Contains("buy") && (action.Contains("resource") || action.Contains("market")))
            {
                string[] t = {
                    $"The Crown is buying {target}! {P("Stockpiling?", "Prices rise.", "Smart investment?", "Markets react.")}",
                    $"Bulk purchases of {target}. {P("War preparation?", "Strategic reserve.", "Merchants profit.")}",
                    $"Royal demand for {target}! {P("Prices soar.", "Sellers celebrate.", "Shortages loom.", "Get yours now.")}",
                    $"The treasury buys {target} in bulk. {P("What do they know?", "Hoarding begins.", "Market frenzy.", "Supply tightens.")}",
                    $"Massive {target} purchases! {P("The Crown's appetite.", "Merchants scramble.", "Prices jump.", "Opportunity?")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("sell") && (action.Contains("resource") || action.Contains("market")))
            {
                string[] t = {
                    $"The Crown is selling {target}! {P("Need gold?", "Prices drop.", "Supply floods market.", "Merchants scramble.")}",
                    $"Selling off {target} reserves. {P("Desperate times?", "Freeing up capital.", "Markets adjust.")}",
                    $"Royal {target} hitting the market. {P("Prices plummet.", "Buyers rejoice.", "Sellers panic.", "Opportunity!")}",
                    $"The treasury liquidates {target}. {P("Raising gold?", "Strategic move?", "Flooding supply.", "Get it cheap now.")}",
                    $"Massive sale of {target}! {P("Market shakes.", "Bargains everywhere.", "What's the rush?", "Something's up.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("trade"))
            {
                string[] t = {
                    $"New trade with {target}. {P("Good for business!", "Merchants rejoice.", "More goods at market.", "Prosperity awaits!")}",
                    $"Trade agreement with {target}! {P("Let's hope it's fair.", "The markets will thrive.", "Smart move by the Crown.")}",
                    $"Trade routes to {target} open! {P("Exotic goods coming.", "Caravans loaded.", "Prices shift.", "New opportunities.")}",
                    $"Commerce with {target} flourishes. {P("Gold flows both ways.", "Everyone benefits.", "Or so they say.", "Time will tell.")}",
                    $"The {P("merchants", "traders", "caravans")} celebrate trade with {target}. {P("Profits!", "New markets!", "Expansion!", "Growth!")}",
                };
                return t[rnd.Next(t.Length)];
            }
            
            // === RULER ===
            else if (action.Contains("festival"))
            {
                string[] t = {
                    $"A festival is announced! {P("Celebrations!", "Food and music!", "The Crown is generous!", "Finally, some joy!")}",
                    $"Festival time! {P("Dancing in the streets.", "Free food I hear.", "Morale booster.", "I'll drink to that!")}",
                    $"The {P("King", "Crown")} declared a festival. {P("Happiness for all!", "What's the occasion?", "I'm not complaining.")}",
                    $"Feasting across the realm! {P("Meat and mead!", "Music fills the air.", "Children laugh.", "Blessed times.")}",
                    $"The Crown throws a celebration! {P("Generosity or distraction?", "Who cares, free ale!", "Dancing tonight!", "Joy spreads.")}",
                    $"Festival declared! {P("Forget your troubles.", "Drink deep.", "Love freely.", "Tomorrow we work. Tonight, we feast!")}",
                    $"A royal celebration! {P("Bonfires burn bright.", "Minstrels play.", "Couples dance.", "Unity in joy.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("disband"))
            {
                string[] t = {
                    $"Troops being disbanded! {P("Peace at last?", "Soldiers go home.", "Less military expense.", "What happened?")}",
                    $"The army shrinks. {P("Risk or wisdom?", "Men return to farms.", "Defense weakens?", "New priorities.")}",
                    $"Demobilization announced. {P("War is over?", "Or budget cuts?", "Soldiers seek work.", "Swords rust.")}",
                    $"The Crown reduces the military. {P("Brave or foolish?", "Peace dividend?", "Enemies watch.", "Change of policy.")}",
                    $"Soldiers sent home. {P("Families reunite!", "Skills wasted?", "Farmhands return.", "A shift in power.")}",
                    $"Army downsizing! {P("Taxes might drop.", "Veterans job-seek.", "Defense concerns.", "The realm adapts.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            
            // === UNION ===
            else if (action.Contains("union") || action.Contains("economic union"))
            {
                string[] t = {
                    $"Economic union with {target}! {P("Shared currency?", "Markets merge.", "Stronger together.", "Trade barriers fall.")}",
                    $"We're joining an economic bloc. {P("Tied fates.", "Prosperity shared.", "Risks too.", "Historic move.")}",
                    $"United economies with {target}! {P("One market now.", "Shared prosperity.", "Combined strength.", "Destiny linked.")}",
                    $"The Crown unites our economy with {target}. {P("Bold experiment.", "Mutual benefit?", "Loss of control?", "Time will tell.")}",
                    $"Economic integration with {target}! {P("Borders blur.", "Trade explodes.", "Cultures mix.", "A new era.")}",
                    $"We merge markets with {target}. {P("Competition increases.", "Efficiency too.", "Winners and losers.", "Adapt quickly.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            
            // === VASSAL ===
            else if (action.Contains("vassal") || action.Contains("puppet"))
            {
                string[] t = {
                    $"{target} is now our vassal! {P("Tribute flows.", "Expansion!", "Power grows.", "They bent the knee.")}",
                    $"We installed a puppet in {target}! {P("Control without conquest.", "Clever politics.", "They serve us now.")}",
                    $"The Crown subjugated {target}. {P("Empire grows.", "Tribute incoming.", "They had no choice.")}",
                    $"{target} bows to us! {P("A new vassal.", "Gold flows our way.", "Their army is ours.", "Dominance.")}",
                    $"A puppet government in {target}! {P("We pull the strings.", "Indirect rule.", "Less cost, same control.", "Smart.")}",
                    $"{target} sworn to our service. {P("Fealty given.", "Tribute promised.", "Protection offered.", "Empire expands.")}",
                    $"The Crown claims {target} as vassal. {P("Knees bent.", "Oaths sworn.", "Gold promised.", "Our reach extends.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("annex"))
            {
                string[] t = {
                    $"{target} has been annexed! {P("Absorbed into the realm!", "No more borders.", "They're us now.", "Complete integration.")}",
                    $"Full annexation of {target}! {P("The map changes.", "One kingdom now.", "History made.")}",
                    $"{target} is fully ours! {P("Complete absorption.", "New citizens.", "Expanded borders.", "Empire grows.")}",
                    $"Annexation complete! {P("Former {target} is now home.", "Unified realm.", "One people.", "No going back.")}",
                    $"The Crown consumes {target}. {P("Total integration.", "Their identity fades.", "Our laws apply.", "Empire swells.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("independence") || action.Contains("grant"))
            {
                string[] t = {
                    $"{target} granted independence! {P("Free at last.", "Too costly to hold?", "Wisdom or weakness?", "They celebrate.")}",
                    $"The Crown released {target}. {P("No more tribute.", "A generous act?", "Strategic withdrawal?")}",
                    $"{target} is free! {P("Self-governance.", "New nation born.", "Ties severed.", "They're on their own.")}",
                    $"Independence for {target}! {P("The Crown lets go.", "Burden lifted.", "Goodwill gesture.", "Or admission of failure?")}",
                    $"The realm shrinks as {target} leaves. {P("Freedom granted.", "Bittersweet farewell.", "May they prosper.", "Or fail alone.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            
            // === BUILD ===
            else if (action.Contains("build") || action.Contains("construct"))
            {
                string[] t = {
                    $"New construction ordered! {P("Jobs!", "Progress.", "The kingdom grows.", "Builders celebrate.")}",
                    $"They're building a {P("barracks", "mine", "windmill", "workshop")}! {P("Investment in the future.", "More infrastructure.", "Growth!")}",
                    $"Construction projects announced. {P("Work for all!", "The realm expands.", "Dust and hammers.")}",
                    $"Building {P("new fortifications", "roads", "bridges", "mills")}! {P("Progress marches on.", "Development!", "The future built today.")}",
                    $"The Crown invests in infrastructure. {P("Smart!", "Jobs created.", "Economy boosted.", "Visible progress.")}",
                    $"Builders summoned! {P("Carpenters", "Masons", "Laborers")} flock to work. {P("Golden opportunity.", "Fair wages promised.", "Progress visible.")}",
                    $"A new {P("barracks rises", "mine opens", "mill turns")}! {P("Production increases.", "Capacity grows.", "The realm strengthens.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            
            // === CULTURE ===
            else if (action.Contains("culture") || action.Contains("spread culture"))
            {
                string[] t = {
                    $"Spreading our culture to {target}! {P("Our ways are best.", "Soft power.", "Unity through culture.", "Assimilation begins.")}",
                    $"Cultural programs launched. {P("Teaching our traditions.", "Pride in heritage.", "Others will learn.")}",
                    $"Our customs spread to {target}! {P("Language taught.", "Traditions shared.", "Identity expands.", "Influence grows.")}",
                    $"Cultural influence in {target}! {P("They adopt our ways.", "Schools open.", "Our festivals celebrated.", "Soft conquest.")}",
                    $"The Crown exports culture. {P("Art flows outward.", "Music spreads.", "Ideas travel.", "Mind over sword.")}",
                    $"Assimilating {target} culturally. {P("They'll be like us soon.", "Unity of thought.", "One people, one culture.", "Peaceful conquest.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("suppress") && action.Contains("culture"))
            {
                string[] t = {
                    $"Suppressing {target} culture! {P("Harsh but necessary?", "Diversity fades.", "Unity through force.", "Dark times for them.")}",
                    $"Cultural suppression ordered. {P("Their ways forbidden.", "One culture only.", "History erased.")}",
                    $"{target} traditions banned! {P("Language forbidden.", "Customs criminalized.", "Identity attacked.", "Resistance grows.")}",
                    $"The Crown crushes {target} culture. {P("Books burned.", "Songs silenced.", "Dances banned.", "Memory persecuted.")}",
                    $"War on {target} identity. {P("Assimilate or suffer.", "Their children forget.", "History rewritten.", "Dark chapter.")}",
                    $"Cultural erasure of {target}. {P("Monuments fall.", "Names change.", "Existence denied.", "Terrible times.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("purge") && action.Contains("culture"))
            {
                string[] t = {
                    $"Cultural purge against {target}! {P("Terrible...", "Erasing a people.", "Dark chapter begins.", "The gods weep.")}",
                    $"They're purging {target} traditions. {P("Monstrous.", "Fear grips those people.", "History will judge.")}",
                    $"Complete cultural annihilation of {target}! {P("No trace left.", "Memory erased.", "As if they never were.", "Evil prevails.")}",
                    $"The Crown exterminates {target} culture. {P("Songs die.", "Language forgotten.", "Identity murdered.", "Ancestors weep.")}",
                    $"Cultural genocide against {target}. {P("History's shame.", "Future's burden.", "How did we allow this?", "Silence is complicity.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            
            // === RELIGION ===
            else if (action.Contains("enforce") && action.Contains("religion"))
            {
                string[] t = {
                    $"State religion enforced! {P("Believe or else.", "One faith for all.", "Temples filled.", "Heresy punished.")}",
                    $"The Crown mandates {target} worship. {P("Pray correctly.", "Forced piety.", "Some resist quietly.")}",
                    $"Mandatory worship! {P("The Crown demands faith.", "Temples overflow.", "Doubt is crime.", "Convert or suffer.")}",
                    $"Religious uniformity enforced. {P("One god, one way.", "Heretics hunted.", "Souls compelled.", "Faith by force.")}",
                    $"The state religion is now law. {P("Other faiths forbidden.", "Pray publicly.", "Show devotion.", "Or face consequences.")}",
                    $"Worship enforced throughout the realm. {P("Temples mandatory.", "Priests empowered.", "Doubt silenced.", "Unity through faith.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("ban") && action.Contains("religion"))
            {
                string[] t = {
                    $"{target} religion banned! {P("Believers in hiding.", "Faith persecuted.", "Dangerous times for them.")}",
                    $"Religious ban on {target}. {P("Pray in secret.", "Temples closed.", "Persecution begins.")}",
                    $"Worshipping {target} is now forbidden! {P("Believers flee.", "Temples razed.", "Faith underground.", "Martyrs made.")}",
                    $"The Crown bans {target} faith. {P("Priests arrested.", "Texts burned.", "Faithful hide.", "Dark times.")}",
                    $"{target} temples closed by decree. {P("Worshippers scatter.", "Icons destroyed.", "Memory persists.", "Faith endures?")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("persecution") || action.Contains("religious persecut"))
            {
                string[] t = {
                    $"Religious persecution ordered! {P("Dark times.", "Blood for faith.", "The pious suffer.", "Inquisitors roam.")}",
                    $"They're hunting {target} believers. {P("Hide your prayers.", "Faith tested.", "Martyrs made.")}",
                    $"Inquisition against {target} faith! {P("Torture chambers fill.", "Confessions extracted.", "Souls broken.", "The gods watch.")}",
                    $"Persecution sweeps the land! {P("Believers hunted.", "Families torn.", "Faith criminalized.", "Terror reigns.")}",
                    $"Religious purges begin. {P("The pious suffer.", "Conviction tested.", "Blood spills for belief.", "History's dark page.")}",
                    $"The Crown crushes {target} worshippers. {P("No mercy.", "No quarter.", "Convert or die.", "Faith under fire.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("tolerance"))
            {
                string[] t = {
                    $"Religious tolerance declared! {P("All faiths welcome.", "Wisdom prevails.", "Peace through acceptance.", "Temples coexist.")}",
                    $"The Crown embraces religious freedom. {P("Progressive!", "Finally.", "Unity in diversity.")}",
                    $"Freedom of worship announced! {P("All gods honored.", "Peace among faiths.", "Tolerance triumphs.", "Enlightened rule.")}",
                    $"Religious pluralism! {P("Many paths, one realm.", "Coexistence.", "Respect for all.", "A new era.")}",
                    $"The Crown allows all faiths. {P("Temples rise for all gods.", "Priests coexist.", "Harmony possible.", "Progressive policy.")}",
                    $"Tolerance prevails! {P("No more persecution.", "Worship freely.", "Believe as you will.", "Peace at last.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            
            // === DEMOGRAPHIC ===
            else if (action.Contains("segregate"))
            {
                string[] t = {
                    $"Segregation ordered for {target}! {P("Divided we stand?", "Separate living.", "Tensions rise.", "Dark policy.")}",
                    $"The Crown segregates {target}. {P("Walls between us.", "Fear breeds policy.", "Unity fractured.")}",
                    $"{target} forced to live apart! {P("Ghettos form.", "Rights limited.", "Distrust grows.", "History warns.")}",
                    $"Separation laws against {target}. {P("Different quarters.", "Marked identities.", "Fear institutionalized.", "Shameful decree.")}",
                    $"The Crown divides us from {target}. {P("Neighbors separated.", "Friendships forbidden.", "Lines drawn.", "Dark precedent.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("integrate"))
            {
                string[] t = {
                    $"Integration policy for {target}! {P("Unity!", "Welcome neighbors.", "Mixing cultures.", "Progressive step.")}",
                    $"The Crown embraces {target}. {P("Diversity is strength.", "One people now.", "Barriers fall.")}",
                    $"{target} welcomed fully! {P("Equal rights.", "Full citizenship.", "Barriers removed.", "A better realm.")}",
                    $"Integration of {target} begins. {P("Schools mix.", "Neighborhoods blend.", "Futures merge.", "Unity grows.")}",
                    $"The Crown unites us with {target}. {P("One people, many origins.", "Strength in diversity.", "Progress!", "Hope rises.")}",
                    $"Embracing {target} as equals. {P("Rights granted.", "Opportunities opened.", "Prejudice challenged.", "Change begins.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("expel") || action.Contains("exile"))
            {
                string[] t = {
                    $"Expelling {target} from the realm! {P("Where will they go?", "Harsh measures.", "Exodus begins.", "Families torn.")}",
                    $"The Crown exiles {target}. {P("Leave or suffer.", "Refugees flood borders.", "Cruel but... necessary?")}",
                    $"{target} ordered to leave! {P("Property seized.", "Lives uprooted.", "Wagons roll out.", "Desperate scenes.")}",
                    $"Mass expulsion of {target}! {P("Roads filled with refugees.", "Other kingdoms close borders.", "Humanitarian crisis.", "History's shame.")}",
                    $"The Crown casts out {target}. {P("Belongings abandoned.", "Memories left behind.", "New homelands sought.", "Diaspora begins.")}",
                    $"Exile for all {target}! {P("Pack what you can.", "Say goodbye forever.", "The road awaits.", "Injustice prevails.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            else if (action.Contains("purge") && !action.Contains("culture"))
            {
                string[] t = {
                    $"A purge against {target}! {P("Blood flows.", "Terrible times.", "History's darkest.", "The gods turn away.")}",
                    $"They're purging {target}... {P("I can't speak of it.", "Hide who you are.", "Monsters among us.", "Pray for them.")}",
                };
                return t[rnd.Next(t.Length)];
            }
            
            // === GENERIC FALLBACK ===
            string[] generic = {
                $"The {P("King", "Crown")} made a decision. {P("I trust their wisdom.", "Let's see what happens.", "Politics...", "Above my understanding.")}",
                $"Word from the palace... {P("changes ahead.", "new orders.", "the usual decrees.", "something's afoot.")}",
                $"The {P("heralds", "criers", "messengers")} announced something. {P("I missed the details.", "Sounded important.", "Who can keep track?")}",
                $"Another royal decree. {P("What now?", "I stopped listening.", "May it be wise.", "The Crown knows best?")}",
                $"Decisions, decisions... the {P("King", "Crown", "Palace")} is busy. {P("As long as we're safe.", "I just work the fields.", "Politics exhaust me.")}",
            };
            return generic[rnd.Next(generic.Length)];
        }
        
        // ============ HIGH TAX PHRASES ============
        private static string BuildHighTaxPhrase(ChatterContext ctx)
        {
            string[] templates = {
                $"These taxes will {P("ruin us", "bleed us dry", "be the death of me", "break our backs")}!",
                $"The {P("King", "Crown", "Ruler")} {P("bleeds us dry", "takes everything", "knows no mercy", "squeezes every coin")} with these taxes{P("!", "...", ". Curse it all!", ". When will it end?")}",
                $"Another {P("coin", "piece of gold", "hard-earned penny")} to the crown... {P("when will it end?", "for what?", "I have nothing left.", "my children go hungry.")}",
                $"Taxed to {P("poverty", "ruin", "the bone", "desperation")}! {P("They say prosperity comes.", "I doubt it.", "What prosperity?", "Lies from the palace.")}",
                $"{P("Hmm", "Sigh", "Ugh", "Gods")}... {P("tax collectors", "the taxman", "those vultures", "the Crown's leeches")} came again{P(".", "...", ". My family starves.", ". Nothing left.")}",
                $"I {P("work", "toil", "labor", "sweat")} from dawn to dusk, and the {P("Crown", "King", "taxman")} takes {P("half", "most", "everything", "the lion's share")}!",
                $"My {P("grandmother", "grandfather", "parents")} never paid taxes this {P("high", "crushing", "unbearable", "ridiculous")}. {P("What changed?", "Times are hard.", "The Crown grows greedy.", "We suffer silently.")}",
                $"The market {P("suffers", "shrinks", "dies slowly")} under these taxes. {P("Who can afford anything?", "Merchants flee.", "Trade dwindles.", "Soon there'll be nothing.")}",
                $"I sold my {P("cow", "tools", "inheritance", "last possession")} to pay the tax. {P("Now what?", "I have nothing.", "The Crown cares not.", "Survival first.")}",
                $"Tax day is {P("tomorrow", "coming", "upon us", "here")}. {P("I dread it.", "Where will I find the coin?", "The collector shows no mercy.", "Hide what you can.")}",
                $"They raised taxes {P("again", "once more", "as expected", "of course")}. {P("As if we weren't suffering enough.", "Curse the Crown's greed.", "My pockets are empty.", "Revolt brews in hearts.")}",
                $"The {P("nobles", "lords", "rich")} pay nothing while {P("we", "the poor", "common folk")} are taxed to {P("oblivion", "breaking point", "starvation")}. {P("Justice?", "Fair?", "I think not.")}",
                $"Every {P("harvest", "season", "moon")}, the tax {P("grows", "increases", "swells")}. {P("When does it end?", "We have limits.", "Something will break.", "The people grumble.")}",
                $"I {P("barely", "hardly", "scarcely")} have enough to {P("eat", "feed my family", "survive")}, and they demand {P("more taxes", "more coin", "more sacrifice")}!",
                $"The {P("children", "young ones", "little ones")} don't understand why there's no {P("food", "bread", "meat")}. {P("I blame the taxes.", "How do I explain?", "The Crown takes all.", "Innocence meets reality.")}",
                $"Tax collectors {P("strut", "parade", "march")} like {P("kings", "lords", "vultures")}. {P("I despise them.", "Necessary evil?", "They enjoy our suffering.", "One day...")}",
                $"My {P("shop", "farm", "trade")} can't survive these taxes. {P("I'll have to close.", "What then?", "Years of work, gone.", "The Crown kills enterprise.")}",
                $"Whispers speak of {P("refusing", "hiding from", "resisting")} the tax. {P("Dangerous talk.", "I understand the temptation.", "The Crown's spies are everywhere.", "But what choice remains?")}",
                $"Even the {P("tavern keeper", "baker", "smith")} complains about taxes now. {P("Everyone suffers.", "Unity in misery.", "The whole town groans.", "Something must give.")}",
                $"{P("Gods", "By the ancestors", "Heavens above")}, have mercy! These taxes {P("crush us", "destroy hope", "mock our labor", "steal our future")}!"
            };
            return templates[rnd.Next(templates.Length)];
        }
        
        // ============ LOW TAX PHRASES ============
        private static string BuildLowTaxPhrase(ChatterContext ctx)
        {
            string[] templates = {
                $"Low taxes! The {P("King", "Crown", "Ruler")} is {P("generous", "wise", "merciful", "kind")}.",
                $"More coin in my pocket! {P("Blessed times.", "I can feed my family.", "The King knows our struggles.", "A rare gift!")}",
                $"Finally, some relief from the taxman! {P("I can breathe.", "Praise the Crown!", "It won't last, but I'll enjoy it.", "Good times!")}",
                $"The taxes are {P("fair", "reasonable", "bearable")} for once. {P("I'm grateful.", "Unexpected kindness.", "May it continue.", "The Crown listened!")}",
                $"My {P("family", "children", "household")} can eat well now that taxes are low. {P("Thank the gods.", "Praise the King.", "A blessing.", "We needed this.")}",
                $"I {P("saved", "kept", "managed to hold")} some coin this season! {P("Low taxes bless us.", "The Crown shows mercy.", "Good policy!", "Long may it last.")}",
                $"The market {P("thrives", "bustles", "flourishes")} with low taxes. {P("Trade blooms!", "Merchants smile.", "Prosperity returns.", "Everyone benefits.")}",
                $"Low taxes mean I can {P("invest in my shop", "buy better tools", "repair my home", "save for the future")}. {P("Smart governance!", "Finally!", "Opportunity knocks.")}",
                $"Neighbors {P("smile", "laugh", "celebrate")} over the low taxes. {P("Community spirit rises.", "Rare good news.", "We feast tonight!", "The King is wise.")}",
                $"For the first time in {P("years", "ages", "my lifetime")}, I don't {P("fear", "dread", "worry about")} tax day. {P("What a feeling!", "Relief washes over me.", "This is how it should be.")}"
            };
            return templates[rnd.Next(templates.Length)];
        }
        
        // ============ DEBT/ECONOMY PHRASES ============
        private static string BuildDebtPhrase(ChatterContext ctx)
        {
            string[] templates = {
                $"The kingdom drowns in {P("debt", "loans", "obligations")}... {P("dark times.", "how will we recover?", "the crown spends too much.", "austerity looms.")}",
                $"I heard the treasury is {P("empty", "depleted", "in crisis", "bare")}. {P("What will become of us?", "Bad omens.", "We must tighten belts.", "Trouble ahead.")}",
                $"Debt upon debt... {P("merchants worry.", "traders flee.", "the economy crumbles.", "confidence falls.")}",
                $"The {P("Crown", "King", "Kingdom")} owes {P("fortunes", "mountains of gold", "more than it has")} to {P("creditors", "foreign banks", "moneylenders")}. {P("Frightening.", "We'll pay for it.", "Interest piles up.", "Default looms.")}",
                $"I fear the kingdom's debt will {P("crush us", "fall on our heads", "mean higher taxes", "ruin trade")}. {P("Hard times coming.", "We're all connected.", "Prepare for the worst.")}",
                $"They say for every coin in the treasury, we owe {P("three", "five", "ten")} more. {P("Madness.", "How did it come to this?", "Poor management.", "War costs.")}",
                $"The {P("banks", "creditors", "lenders")} grow nervous about our debt. {P("Credit dries up.", "Interest rises.", "Faith wavers.", "Dark clouds gather.")}",
                $"National debt? {P("I don't understand it fully, but...", "They explained it at the tavern:", "The scholars say:")} it means {P("trouble", "hard times", "sacrifice ahead")}.",
                $"My {P("father", "grandfather")} said: '{P("Never borrow more than you can repay.", "Debt is slavery.", "Live within means.")}' The {P("Crown", "Kingdom")} should listen.",
                $"Markets are {P("jittery", "unstable", "panicking")} because of the debt. {P("My savings feel worthless.", "Prices swing wildly.", "Trade suffers.", "Fear spreads.")}",
                $"The {P("wise", "old", "learned")} say debt this high leads to {P("collapse", "revolution", "crisis", "ruin")}. {P("History teaches.", "I hope they're wrong.", "Brace yourselves.")}",
                $"I {P("converted", "changed", "traded")} my savings to {P("goods", "grain", "something real")}. {P("Debt makes currency worthless.", "I don't trust the treasury.", "Safety in tangibles.")}",
                $"The {P("children", "youth")} will inherit this debt. {P("Poor souls.", "We failed them.", "They'll curse us.", "An unfair burden.")}",
                $"Every {P("market", "shop", "trader")} talks about the kingdom's debt. {P("Fear is everywhere.", "Confidence evaporates.", "Everyone's worried.", "Dark conversations.")}",
                $"Debt so high the {P("sun", "stars", "ancients")} couldn't count it. {P("Exaggeration? Maybe not.", "We're in deep.", "The Crown must act.", "Prayer won't fix this.")}"
            };
            return templates[rnd.Next(templates.Length)];
        }
        
        // ============ PROSPEROUS PHRASES ============
        private static string BuildProsperousPhrase(ChatterContext ctx)
        {
            string kingdom = ctx.KingdomName ?? "our realm";
            string[] templates = {
                $"{kingdom} {P("prospers", "thrives", "flourishes", "blooms")}! {P("Blessed times!", "The gods smile upon us.", "Life is good.", "Golden age!")}",
                $"The markets overflow! {P("Prosperity!", "Golden age!", "We are blessed.", "Trade booms!", "Fortune favors us!")}",
                $"Wealth flows through {kingdom}. {P("Long may it last!", "I've never seen such times.", "Praise the Crown!", "Let us be grateful.")}",
                $"Good times! {P("My purse is full.", "Food is plenty.", "The children smile.", "Work is plentiful.", "Joy abounds!")}",
                $"I {P("bought", "afforded", "purchased")} a {P("new tool", "better home", "fine garment", "luxury")} today! {P("Prosperity is real!", "Hard work pays off.", "Good governance rewards.", "Blessed am I.")}",
                $"The {P("granaries", "warehouses", "stores")} are {P("full", "overflowing", "bursting")}! {P("No famine this year.", "Security feels good.", "We prepared well.", "Abundance surrounds us.")}",
                $"Merchants from {P("far lands", "distant kingdoms", "across the seas")} flock to {kingdom}. {P("We're famous!", "Trade capital!", "Opportunity abounds.", "Success attracts success.")}",
                $"My {P("children", "family", "loved ones")} want for nothing. {P("This is happiness.", "Grateful every day.", "May it continue.", "The gods are kind.")}",
                $"Even the {P("beggars", "poor", "unfortunate")} eat well in prosperous {kingdom}. {P("Charity flows.", "Wealth lifts all.", "Nobody starves.", "Community thrives.")}",
                $"I {P("saved", "invested", "stored away")} more coin this year than ever. {P("Security at last!", "The future looks bright.", "Smart governance pays.", "Hard work rewarded.")}",
                $"New {P("buildings", "shops", "homes")} rise everywhere. {P("Growth is visible!", "Progress marches on.", "The kingdom expands.", "Opportunity for all.")}",
                $"The {P("festivals", "celebrations", "feasts")} are grander each year. {P("Joy overflows!", "We've earned this.", "Life is beautiful.", "Community bonds strengthen.")}",
                $"Prosperity like this is {P("rare", "precious", "a gift")}. {P("Don't take it for granted.", "Enjoy it while we can.", "May wisdom preserve it.", "Gratitude fills my heart.")}",
                $"The {P("old-timers", "elders", "veterans")} say they've never seen {kingdom} this {P("rich", "abundant", "thriving")}. {P("Historic times!", "We live in legend.", "Cherish this moment.")}",
                $"Work {P("aplenty", "everywhere", "for all who seek it")} and fair wages too! {P("The Crown's policies work.", "Initiative rewarded.", "No excuse for laziness now.", "Golden era indeed.")}"
            };
            return templates[rnd.Next(templates.Length)];
        }
        
        // ============ PERSONAL STATE PHRASES ============
        private static string BuildHungryPhrase(ChatterContext ctx)
        {
            string[] templates = {
                $"{P("Hungry", "Starving", "So hungry", "Famished")}... {P("need food.", "where's the food?", "my stomach aches.", "can't think straight.")}",
                $"When did I last {P("eat", "have a meal", "taste bread", "fill my belly")}? {P("I can't remember.", "Too long ago.", "Ages...", "Days, I think.")}",
                $"Food... {P("I need food.", "please, anything.", "just a morsel.", "my kingdom for a meal.")}",
                $"My stomach {P("growls", "rumbles", "protests", "aches")} louder than {P("thunder", "drums", "my thoughts")}. {P("Must find food.", "So empty.", "The hunger consumes me.")}",
                $"The {P("bread", "meat", "fruit")} at the market smells {P("divine", "incredible", "torturous")}. {P("If only I had coin.", "I'll just smell it.", "Torture for the hungry.")}",
                $"I'd {P("trade", "give", "sacrifice")} anything for a {P("hot meal", "full belly", "decent supper")} right now.",
                $"The children {P("ask", "cry", "beg")} for food. {P("My heart breaks.", "What can I do?", "I have nothing.", "The larder is empty.")}",
                $"Hunger makes everything {P("harder", "dimmer", "worse")}. {P("Can't focus.", "Strength fades.", "Hope too.", "Body and spirit suffer.")}",
                $"I saw {P("rats", "scraps", "leftovers")} and actually considered... {P("No, not yet.", "How far I've fallen.", "Hunger drives us mad.", "Pride is expensive.")}",
                $"A {P("feast", "banquet", "meal")} in my dreams, {P("dust", "emptiness", "nothing")} when I wake. {P("Cruel reality.", "Hope keeps me going.", "Tomorrow might be better.")}"
            };
            return templates[rnd.Next(templates.Length)];
        }
        
        private static string BuildInjuredPhrase(ChatterContext ctx)
        {
            string[] templates = {
                $"My {P("wounds", "injuries", "body")} {P("ache", "burn", "won't heal", "throb")}...",
                $"{P("Ouch", "Agh", "Nngh", "Argh")}... {P("the pain.", "it hurts.", "I need rest.", "this is agony.")}",
                $"I've seen {P("better days", "worse", "battle")}. {P("I'll survive.", "Barely.", "Just need rest.", "Scars tell stories.")}",
                $"Every {P("step", "movement", "breath")} reminds me of the {P("wound", "injury", "damage")}. {P("Patience.", "Healing takes time.", "The healer said rest.")}",
                $"The {P("healer", "priest", "doctor")} says I'll {P("recover", "heal", "be fine")}, but {P("it hurts now.", "the pain is real.", "waiting is hard.")}",
                $"Blood {P("stains", "marks", "soaks")} my {P("clothes", "bandages", "garments")}. {P("Battle's mark.", "Price of survival.", "Could've been worse.")}",
                $"I {P("push through", "ignore", "endure")} the pain. {P("Work must continue.", "Can't afford rest.", "Duty calls.", "The family needs me.")}",
                $"My {P("arm", "leg", "side", "back")} {P("screams", "protests", "refuses to cooperate")}. {P("Cursed injury.", "Time will heal.", "I hope.")}",
                $"The {P("scar", "wound", "mark")} will {P("fade", "heal", "tell a tale")}. {P("Eventually.", "I hope.", "For now, pain.")}",
                $"Injured but {P("alive", "breathing", "standing")}. {P("That's what matters.", "Could be worse.", "I'm lucky.", "Grateful for that much.")}"
            };
            return templates[rnd.Next(templates.Length)];
        }
        
        private static string BuildMiserablePhrase(ChatterContext ctx)
        {
            string[] templates = {
                $"{P("Life is", "Everything is", "This world is")} {P("misery", "suffering", "pain", "darkness")}...",
                $"Why go on? {P("Nothing matters.", "What's the point?", "Dark thoughts...", "Emptiness consumes me.")}",
                $"I've lost {P("everything", "all hope", "my way", "myself")}...",
                $"{P("Darkness", "Despair", "Gloom", "Shadow")} everywhere I look...",
                $"My {P("soul", "heart", "spirit")} feels {P("empty", "broken", "dead", "hollow")}. {P("Is this life?", "What purpose remains?", "The void stares back.")}",
                $"I {P("smiled", "laughed", "felt joy")} once. {P("A distant memory.", "Hard to believe now.", "Was that real?", "Another life.")}",
                $"The {P("weight", "burden", "heaviness")} of {P("existence", "living", "everything")} {P("crushes me", "drags me down", "suffocates")}. {P("....", "No escape.", "Endure.")}",
                $"They say {P("tomorrow", "things", "life")} will be better. {P("They always say that.", "I don't believe it.", "Empty words.", "Hope is cruel.")}",
                $"{P("Sleep", "Oblivion", "Rest")} is my only {P("escape", "comfort", "solace")}. {P("Waking is the curse.", "Dreams mock me too.", "Temporary peace.")}",
                $"What did I do to deserve this {P("misery", "fate", "existence")}? {P("The gods are silent.", "No answers come.", "Punishment for past sins?", "Randomness of life.")}"
            };
            return templates[rnd.Next(templates.Length)];
        }
        
        private static string BuildHappyPhrase(ChatterContext ctx)
        {
            string[] templates = {
                $"Life is {P("wonderful", "beautiful", "amazing", "grand")}! {P("*hums*", "*smiles*", "*skips*", "")}",
                $"What a {P("glorious", "beautiful", "perfect", "lovely")} day!",
                $"I feel {P("blessed", "lucky", "joyful", "grateful")} today!",
                $"{P("Happiness", "Joy", "Bliss", "Contentment")} fills my heart!",
                $"Everything is {P("great", "perfect", "just right", "wonderful")}!",
                $"I could {P("sing", "dance", "shout", "laugh")} from {P("joy", "happiness", "pure bliss")}!",
                $"The {P("sun", "sky", "world")} seems {P("brighter", "kinder", "more beautiful")} today. {P("I love life!", "Blessed am I.", "What a time to be alive!")}",
                $"My {P("heart", "soul", "spirit")} is {P("light", "free", "soaring")}! {P("Pure joy!", "No worries!", "This is living!")}",
                $"I woke up {P("smiling", "grateful", "eager")} today! {P("Rare gift.", "Best feeling.", "May it last.")}",
                $"{P("Gratitude", "Thankfulness", "Appreciation")} overflows in me. {P("Life is good.", "I'm lucky.", "Blessed beyond words.")}",
                $"The little things bring such {P("joy", "happiness", "pleasure")}: {P("a warm meal", "a kind word", "sunshine", "a friendly face")}.",
                $"I {P("hugged", "thanked", "appreciated")} my {P("family", "friends", "loved ones")} today. {P("Love is real.", "Connection matters.", "Bonds strengthen.")}",
                $"Nothing can {P("bring me down", "dampen my spirits", "shadow this joy")} today!",
                $"The {P("birds", "children", "music")} {P("sing", "play", "echo")} my {P("happiness", "mood", "joy")}. {P("Harmony!", "Perfect moment.", "Remember this feeling.")}",
                $"If I {P("died", "passed", "left")} tomorrow, I'd go {P("happy", "content", "fulfilled")}. {P("Life has been good.", "No regrets.", "Grateful for everything.")}"
            };
            return templates[rnd.Next(templates.Length)];
        }
        
        // ============ WORLD STATE PHRASES ============
        private static string BuildGlobalWarPhrase(ChatterContext ctx)
        {
            string[] templates = {
                $"Dark times... {P("war everywhere.", "the world burns.", "nations clash.", "blood soaks every land.")}",
                $"So many {P("wars", "conflicts", "battles", "struggles")} across the {P("lands", "world", "kingdoms", "realms")}...",
                $"When will there be {P("peace", "quiet", "rest", "calm")} in this world?",
                $"The {P("earth", "world", "realm")} is soaked in {P("blood", "conflict", "strife")}. {P("Will it ever end?", "History repeats.", "Humanity's curse.")}",
                $"Every {P("kingdom", "nation", "realm")} seems at {P("war", "conflict", "odds")} with another. {P("Madness.", "The gods weep.", "Where's the wisdom?")}",
                $"I hear of {P("battles", "sieges", "massacres")} from every direction. {P("No safe haven.", "The world ablaze.", "Fear spreads.")}",
                $"Global {P("war", "conflict", "strife")} makes {P("trade", "travel", "peace")} {P("impossible", "dangerous", "a dream")}. {P("We suffer too.", "Isolation grows.", "Borders close.")}",
                $"My {P("grandfather", "ancestors")} spoke of peaceful times. {P("Hard to imagine now.", "Legends, surely.", "Will they return?")}",
                $"Refugees from {P("everywhere", "all corners", "distant lands")} flee to anywhere {P("safe", "peaceful", "untouched")}. {P("There's nowhere.", "We're next?", "Humanity wanders.")}",
                $"The {P("wise", "scholars", "seers")} say this much {P("war", "conflict", "bloodshed")} hasn't happened in {P("centuries", "ages", "recorded history")}. {P("Apocalyptic.", "We live in historic horror.", "May it end.")}"
            };
            return templates[rnd.Next(templates.Length)];
        }
        
        // ============ WORK/DAILY PHRASES ============
        private static string BuildWorkPhrase(ChatterContext ctx)
        {
            string[] templates = {
                $"Back to {P("work", "the grind", "labor", "toil")}...",
                $"Another day, another {P("coin", "task", "struggle", "effort")}.",
                $"The {P("work", "toil", "labor", "effort")} never ends...",
                $"{P("Working hard", "Busy", "No rest", "Toiling away")}. {P("As always.", "Such is life.", "What else?", "")}",
                $"My {P("hands", "back", "body")} {P("ache", "protest", "know work")} from honest labor. {P("Good tired.", "Price of living.", "I earn my keep.")}",
                $"The {P("shop", "field", "forge", "market")} awaits. {P("Duty calls.", "Customers wait.", "Work defines me.", "Let's go.")}",
                $"I {P("love", "tolerate", "endure")} my work. {P("Most days.", "It pays.", "Could be worse.", "Keeps me fed.")}",
                $"So much to do, so little {P("time", "energy", "daylight")}. {P("Priorities.", "One thing at a time.", "Deep breath.")}",
                $"The {P("boss", "master", "foreman")} expects more every day. {P("I try my best.", "Never satisfied.", "Demanding times.", "I'll manage.")}",
                $"Work is {P("life", "purpose", "survival")} in these times. {P("No complaints.", "Grateful to have it.", "Many have none.")}"
            };
            return templates[rnd.Next(templates.Length)];
        }
        
        private static string BuildDailyPhrase(ChatterContext ctx)
        {
            string[] templates = {
                $"Just another {P("day", "morning", "evening")} in {P("paradise", "the realm", "our village")}.",
                $"Time for {P("breakfast", "lunch", "supper", "a meal")}. {P("I hope.", "If there's food.", "The best part of the day.")}",
                $"The {P("market", "square", "tavern")} is {P("busy", "quiet", "normal")} today.",
                $"I need to {P("fix", "repair", "mend")} {P("the roof", "my tools", "the fence")}... {P("eventually.", "today maybe.", "someday.")}",
                $"Did I {P("lock", "close", "secure")} the {P("door", "gate", "shop")}? {P("...yes, surely.", "Better check.", "Memory fades.")}",
                $"The {P("children", "youth", "young ones")} are {P("playing", "learning", "growing")} so fast. {P("Time flies.", "Cherish each moment.", "The future.")}",
                $"I should visit {P("my mother", "the healer", "an old friend")} soon. {P("Time escapes me.", "Life gets busy.", "Tomorrow perhaps.")}",
                $"What to {P("eat", "cook", "prepare")} tonight? {P("Same as always.", "Something special?", "Whatever's left.")}",
                $"The {P("neighbors", "townsfolk", "locals")} seem {P("friendly", "distant", "normal")} lately. {P("Community matters.", "Everyone's busy.", "Life goes on.")}",
                $"{P("Morning", "Evening", "Afternoon")} routines... {P("comforting.", "mundane.", "keep me sane.", "structure is good.")}"
            };
            return templates[rnd.Next(templates.Length)];
        }
        
        // ============ GOSSIP PHRASES ============
        private static string BuildGossipPhrase(ChatterContext ctx)
        {
            string kingdom = ctx.KingdomName ?? "the kingdom";
            string[] templates = {
                $"Did you hear about {P("the new decree", "the scandal", "what happened", "the rumor")}?",
                $"They say {P("the King", "someone important", "a noble", "the advisor")} {P("is ill", "made a mistake", "has secrets", "is in trouble")}...",
                $"Rumors from {P("the palace", "the tavern", "the market", "travelers")}... {P("interesting.", "can't say more.", "you didn't hear it from me.", "take it with salt.")}",
                $"Have you {P("heard", "seen", "noticed")}? {P("Strange things afoot.", "Something's happening.", "Change is coming.", "Whispers grow louder.")}",
                $"The {P("baker", "smith", "healer", "merchant")} was saying... {P("well, never mind.", "I shouldn't repeat it.", "you know how they talk.", "interesting theories.")}",
                $"I shouldn't gossip but... {P("did you hear?", "word is...", "between us...", "rumor has it...")}",
                $"My {P("neighbor", "cousin", "friend")} claims {P("the oddest thing", "unbelievable news", "quite the tale")}. {P("I'm skeptical.", "Could be true!", "Who knows?")}",
                $"They're talking in the {P("tavern", "market", "streets")} about {P("politics", "the Crown's decisions", "unusual events")}. {P("Ears everywhere.", "Be careful.", "Opinions differ.")}",
                $"A {P("traveler", "merchant", "stranger")} brought news of {P("distant lands", "other kingdoms", "strange happenings")}. {P("Fascinating!", "Hard to verify.", "The world is vast.")}",
                $"Keep this between us: {P("I heard...", "word is...", "they say...")} {P("well, you know.", "use your imagination.", "walls have ears.", "maybe nothing.")}"
            };
            return templates[rnd.Next(templates.Length)];
        }
        
        // ============ WEATHER PHRASES ============
        private static string BuildWeatherPhrase(ChatterContext ctx)
        {
            string[] templates = {
                $"Nice {P("weather", "day", "sky")} today. {P("Enjoy it while it lasts.", "Perfect for work.", "The gods smile.", "")}",
                $"Looks like {P("rain", "storm", "clouds")} coming. {P("Hope the crops like it.", "Better head inside.", "Nature's way.")}",
                $"The {P("sun", "warmth", "breeze")} feels {P("wonderful", "blessed", "perfect")} today!",
                $"{P("Cold", "Chilly", "Brisk")} {P("morning", "wind", "air")} today. {P("Bundle up!", "Winter approaches.", "Hot drink needed.")}",
                $"The {P("sky", "heavens", "clouds")} look {P("ominous", "beautiful", "interesting")} today. {P("Nature speaks.", "Weather changes.", "Take note.")}",
                $"I {P("love", "enjoy", "appreciate")} days like these. {P("Perfect temperature.", "Not too hot, not too cold.", "Weather's blessing.")}",
                $"The {P("farmers", "crops", "land")} {P("need", "crave", "welcome")} this {P("rain", "sun", "weather")}. {P("Harvest depends on it.", "Nature provides.", "Balance is key.")}",
                $"Weather like this reminds me of {P("childhood", "simpler times", "my youth")}. {P("Nostalgia.", "Good memories.", "Time flies.")}"
            };
            return templates[rnd.Next(templates.Length)];
        }
        
        // ============ PHILOSOPHY PHRASES ============
        private static string BuildPhilosophyPhrase(ChatterContext ctx)
        {
            string[] templates = {
                $"What is the {P("meaning", "purpose", "point")} of {P("life", "existence", "all this")}? {P("I wonder.", "Deep thoughts.", "No answers come.")}",
                $"They say {P("time heals", "what goes around comes around", "nothing lasts forever")}. {P("I hope so.", "Wise words.", "We'll see.")}",
                $"Are we {P("free", "destined", "just pawns")}? {P("Philosophy puzzles me.", "Deep question.", "The scholars debate.")}",
                $"The {P("cycle", "wheel", "flow")} of {P("life", "history", "fate")} continues. {P("Unchanging.", "We're part of it.", "Round and round.")}",
                $"What will {P("future generations", "our children", "history")} say about us? {P("I wonder.", "Hopefully kind.", "We do our best.")}",
                $"Is this {P("all there is", "the best it gets", "reality")}? {P("Questions without answers.", "Faith varies.", "Live while you can.")}",
                $"The {P("wise", "ancients", "philosophers")} wrote of {P("virtue", "balance", "truth")}. {P("Hard to practice.", "Noble ideals.", "We strive.")}",
                $"Every {P("action", "choice", "word")} ripples through {P("time", "fate", "others' lives")}. {P("Weight of existence.", "Choose wisely.", "Consequences matter.")}"
            };
            return templates[rnd.Next(templates.Length)];
        }
        
        // ============ TRAIT-BASED PHRASES ============
        private static string BuildGreedyPhrase(ChatterContext ctx)
        {
            string[] templates = {
                $"Gold... {P("never enough gold.", "more gold.", "shiny, beautiful gold.", "I need more.")}",
                $"What's that worth? {P("Could I sell it?", "Hmm, valuable.", "I'll make an offer.", "Everything has a price.")}",
                $"I {P("counted", "polished", "admired")} my coins {P("twice", "again", "obsessively")} today. {P("Worth it.", "They're beautiful.", "Can never be too careful.")}",
                $"More {P("wealth", "gold", "coin")} means more {P("power", "security", "options")}. {P("Simple truth.", "The only truth.", "I understand this.")}",
                $"I'd {P("trade", "sell", "sacrifice")} almost anything for {P("profit", "gold", "wealth")}. {P("Almost.", "Business is business.", "Don't judge me.")}",
                $"They call me {P("greedy", "avaricious", "miser")}. I call it {P("practical", "smart", "prepared", "ambitious")}.",
                $"My {P("treasure", "savings", "hoard")} grows daily. {P("As it should.", "Never enough.", "Security at last?")}",
                $"The {P("wealthy", "rich", "prosperous")} understand: {P("gold solves problems.", "money is power.", "poverty is weakness.")}",
            };
            return templates[rnd.Next(templates.Length)];
        }
        
        private static string BuildWisePhrase(ChatterContext ctx)
        {
            string[] templates = {
                $"These times shall pass, as all things do. {P("Patience.", "Such is the nature of existence.", "History teaches.")}",
                $"The {P("wise", "learned", "patient")} observe before {P("acting", "speaking", "judging")}. {P("Lesson from my youth.", "Still learning.", "Wisdom comes slow.")}",
                $"Every {P("hardship", "challenge", "trial")} carries a {P("lesson", "gift", "opportunity")}. {P("If we look.", "Hidden wisdom.", "Growth through pain.")}",
                $"I've seen {P("empires", "kings", "fortunes")} {P("rise and fall", "come and go", "transform")}. {P("Nothing lasts.", "Perspective matters.", "Gratitude helps.")}",
                $"What seems {P("disaster", "terrible", "hopeless")} today may prove {P("blessing", "growth", "necessary")} tomorrow.",
                $"The {P("youth", "young", "inexperienced")} rush; the {P("wise", "old", "seasoned")} consider. {P("Both have value.", "Balance is key.", "I've been both.")}",
                $"Knowledge is {P("gathered", "earned", "accumulated")}; wisdom is {P("lived", "suffered", "integrated")}. {P("Different paths.", "Both matter.", "Experience teaches.")}",
                $"Nature teaches {P("patience", "cycles", "balance")} to those who {P("listen", "observe", "attend")}. {P("I try to learn.", "Slow student.", "Always more to see.")}"
            };
            return templates[rnd.Next(templates.Length)];
        }
        
        private static string BuildAggressivePhrase(ChatterContext ctx)
        {
            string[] templates = {
                $"I'd {P("fight", "battle", "crush")} them all if I could! {P("Give me the chance!", "Cowards hide.", "My fists itch.")}",
                $"Violence {P("solves problems", "answers questions", "speaks clearly")}. {P("Proven fact.", "Don't disagree.", "Results matter.")}",
                $"Who needs {P("diplomacy", "words", "talk")} when you have {P("strength", "steel", "fists")}!",
                $"I {P("see red", "feel rage", "hunger for battle")} when provoked. {P("Fair warning.", "Control is hard.", "I'm working on it.")}",
                $"The {P("weak", "soft", "peaceful")} don't survive. {P("Nature's law.", "Harsh truth.", "I adapt.")}",
                $"Point me at the {P("enemy", "problem", "obstacle")} and watch me {P("destroy it", "triumph", "prevail")}!",
                $"Talking is for those too {P("weak", "scared", "soft")} to {P("fight", "act", "win")}.",
                $"My {P("temper", "blood", "instincts")} run {P("hot", "wild", "fierce")}. {P("Can't help it.", "It's who I am.", "Useful in battle.")}"
            };
            return templates[rnd.Next(templates.Length)];
        }
        
        // ============ ROLE-BASED PHRASES ============
        private static string BuildSoldierPhrase(ChatterContext ctx)
        {
            string[] templates = {
                $"Training {P("never ends", "is tough", "makes us strong", "defines us")}.",
                $"For the {P("Kingdom", "Crown", "glory", "realm")}!",
                $"Ready for {P("battle", "anything", "orders", "war", "combat")}.",
                $"My {P("blade", "sword", "weapon")} is {P("sharp", "ready", "thirsting")}. {P("As am I.", "Always prepared.", "Bring the fight.")}",
                $"The {P("drill sergeant", "captain", "commander")} says jump, I {P("jump", "obey", "ask how high")}. {P("Discipline.", "Chain of command.", "Soldier's life.")}",
                $"I've {P("killed", "fought", "bled")} for this {P("kingdom", "crown", "land")}. {P("No regrets.", "Duty is duty.", "Honor demands.")}",
                $"Waiting for {P("orders", "battle", "action")} is {P("tedious", "necessary", "part of service")}. {P("Patience tested.", "Ready when called.", "Vigilance.")}",
                $"The {P("barracks", "camp", "garrison")} is {P("home", "familiar", "all I know")} now. {P("Soldier's life.", "I chose this.", "Brothers all around.")}",
                $"I {P("march", "fight", "die")} for {P("home", "family", "the people")}. {P("That's what matters.", "Noble purpose.", "Worth the sacrifice.")}",
                $"Scars tell {P("stories", "tales", "history")}. Mine say {P("survivor", "warrior", "veteran")}."
            };
            return templates[rnd.Next(templates.Length)];
        }
        
        private static string BuildKingPhrase(ChatterContext ctx)
        {
            string kingdom = ctx.KingdomName ?? "the realm";
            string[] templates = {
                $"The {P("crown", "throne", "scepter")} is {P("heavy", "demanding", "lonely")}. {P("Such is the price of rule.", "I bear it gladly.", "Duty before self.")}",
                $"{kingdom}'s {P("fate", "future", "destiny")} rests on my {P("shoulders", "decisions", "wisdom")}. {P("No pressure.", "I'm prepared.", "The gods guide me.")}",
                $"Every {P("decision", "choice", "decree")} affects {P("thousands", "the realm", "generations")}. {P("Weighty responsibility.", "I choose carefully.", "May wisdom prevail.")}",
                $"They {P("cheer", "praise", "bow")} today, but {P("tomorrow", "fickle crowds", "opinions change")}... {P("Such is ruling.", "I don't rule for applause.", "The work continues.")}",
                $"A {P("good king", "wise ruler", "true sovereign")} serves the {P("people", "realm", "common good")}. {P("That's my aim.", "Always learning.", "Heavy duty.")}",
                $"I've sacrificed {P("much", "everything", "myself")} for {kingdom}. {P("Worth it.", "Gladly.", "The realm endures.")}",
                $"Advisors {P("counsel", "suggest", "argue")}, but I {P("decide", "bear responsibility", "choose")}. {P("The burden of the crown.", "Lonely at the top.", "Final call is mine.")}",
                $"May {P("history", "the gods", "my descendants")} judge me {P("fairly", "kindly", "wisely")}. {P("I tried my best.", "Intentions were good.", "Results will speak.")}"
            };
            return templates[rnd.Next(templates.Length)];
        }
        
        // ============ GENERIC PHRASES ============
        private static string GetGenericPhrase()
        {
            string[] templates = {
                "Nice weather today.",
                "Just another day...",
                "Hmm, interesting times.",
                "Life goes on.",
                "*yawns*",
                "...",
                "Have you seen the market lately?",
                "The roads are busy today.",
                "I wonder what tomorrow brings.",
                "Is it just me or...?",
                "Heard any news?",
                "Same old, same old.",
                "Could use a drink.",
                "My feet hurt.",
                "What's for dinner?",
                "The children grow so fast.",
                "Time flies.",
                "I miss simpler times.",
                "Who knows what the future holds?",
                "One day at a time.",
            };
            return templates[rnd.Next(templates.Length)];
        }
        
        // ============ HELPERS ============
        private static string P(params string[] options) => options[rnd.Next(options.Length)];
        
        private static string ApplyVariation(string phrase)
        {
            if (rnd.Next(5) == 0)
            {
                string[] starters = { "Hmm, ", "Well, ", "Say, ", "Ah, ", "You know, ", "Listen, ", "" };
                if (phrase.Length > 0) phrase = starters[rnd.Next(starters.Length)] + char.ToLower(phrase[0]) + phrase.Substring(1);
            }
            return phrase;
        }
    }
}
