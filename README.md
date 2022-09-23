## One Day RPG

This is a simple RPG battle game implementation where player can have a collection of heroes, select 3 of those heroes for battle and battle with an enemy.

**Game should have a hero structure**

 - Heroes can be generated randomly or hard-coded
 - Hero attributes are name, health, attack power, experience and level.
 - Health attribute will only be used in battle. Others will be hero’s static attributes.

**Game should have a hero collection structure to keep all collected heroes and select them for battle.**

- A player can have 10 different heroes at maximum. 
- Initially collection will only contain 3 heroes. 
- Make sure heroes can be distinguished from each other visually (for example different colored boxes).

**Battle system should work like:**

- Player should select at least 3 heroes to enter a battle.
- After 3 heroes selected game should allow user start a battle.
- When battle initiated, game will display battlefield. 3 heroes on left and 1 enemy on right side.
- Player will tap on which hero to attack. That hero will attack to enemy, then enemy will attack to a random enemy. Player can select any enemy on battlefield at any time of battle, player can use the same hero or different heroes every turn.
- When player tapped on a hero to attack, hero will fly to enemy, do damage and fly back to it’s initial position. This will be same for enemy also.
- Only 1 hero or enemy attack at a time. So game should wait the attack action to be completed before accepting any other input from player.
- Battle finishes when all heroes’ health attributes are 0 or enemy’s health attribute is 0.
- Every battle won will increase alive heroes’ (health > 0) experience attribute +1.
- When battle finishes status of the battle will be displayed on battlefield (win or loose). And after that game will go back to hero selection screen.
- Every 5th enemy battle (does not matter win or loose) will give a random hero until player’s hero count reaches to 10.
- Every 5 experience points will increase hero’s level +1 automatically. Level increase will increase hero’s attack point and health attributes by 10%.

**There are some UI requirements we expect to see in the gameplay.**

- On hero selection screen if a hero is tapped, it means it is selected. Player should be informed visually that hero is selected (border might be bold or color could change for example).
- If a hero is tapped and hold for 3 seconds it will display info popup on both hero selection and battlefield screens.
- On battlefield health of heroes and enemy should be displayed on top of their visual representations.
- Attacks values and attribute increases should be displayed to user (for example as fading out numbers on screen).
- When battle finishes it should display player status of the battle (win or loose) and a button to return back to hero selection screen.

**All data should be data should persist between sessions. After restarting the game player should continue playing the game from where it is left.**
