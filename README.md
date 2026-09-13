# Yes Chef! — Game Design & Implementation README

## 1. Game Overview

**Yes Chef!** is a short, fast-paced kitchen order-management game.

The player works inside a single 3D kitchen, prepares ingredients, fulfils customer orders, and tries to achieve the highest possible score during a **3-minute session**.

The game uses a fixed top-down **perspective camera** and simple 3D visuals.

---

## 2. Core Gameplay Loop

1. Start a 3-minute kitchen session.
2. Four customer orders are active at the beginning.
3. Take ingredients from the refrigerator.
4. Prepare ingredients at the appropriate station.
5. Deliver prepared ingredients to the correct customer window.
6. Complete the order.
7. Receive the calculated score.
8. The completed order disappears.
9. After **5 seconds**, a new order appears at the same window.
10. Continue until the 3-minute session ends.
11. The final score is compared against the persistent high score.

A maximum of **4 orders** are active at a time.

---

## 3. Kitchen Layout

The kitchen contains:

- Refrigerator
- Chopping table
- Stove
- Four customer windows
- Trash station
- Player
- Fixed camera
- UI/HUD

The four customer windows are represented by a single **CustomerWall** object with four window points.

### Customer Wall Architecture

```text
CustomerWall
├── WindowPoint_01
├── WindowPoint_02
├── WindowPoint_03
└── WindowPoint_04
```

`OrderManager` is a separate game-system object and references the scene `CustomerWall`.

This keeps order management separate from the physical kitchen/wall prefab.

---

# 4. Ingredient Design

There are three ingredient types:

| Ingredient | Preparation | Time | Score |
|---|---|---:|---:|
| Vegetable | Chopping | 2 sec | 20 |
| Cheese | None | 0 sec | 10 |
| Meat | Cooking | 6 sec | 30 |

Each ingredient has two runtime states:

```text
Raw
Prepared
```

Cheese does not require preparation and can be delivered immediately.

Vegetables must be chopped before delivery.

Meat must be cooked before delivery.

---

# 5. Ingredient Mesh / Visual Design

The ingredient meshes are intentionally **simple, readable 3D game objects** rather than highly detailed realistic food models.

The goal is that each ingredient can be recognized immediately from the fixed top-down camera.

## Vegetable Mesh

The vegetable uses a simple low-detail food/vegetable-style mesh.

Visual requirements:

- Compact shape
- Clearly recognizable as a vegetable
- Simple geometry is acceptable
- Bright, readable material
- Small enough to comfortably fit in the player's hand
- Prepared/cooked version should look visibly different from the raw version

The vegetable prefab contains the gameplay component and its visual representation.

Example structure:

```text
Vegetable
├── IngredientInstance
├── IngredientVisual
├── RawVisual
└── PreparedVisual
```

The `IngredientVisual` component switches between the raw and prepared visual.

---

## Cheese Mesh

The cheese is represented as a simple compact cheese-style mesh.

Visual requirements:

- Clearly different from vegetable and meat
- Compact block/wedge-like silhouette
- Simple geometry is acceptable
- Readable from the fixed camera
- No preparation animation is required
- Cheese can be delivered directly after pickup

The cheese also has its own raw/delivered UI artwork.

---

## Meat Mesh

The meat uses a simple steak/meat-style mesh.

Visual requirements:

- Clearly recognizable as meat
- Compact enough for the player's hand
- Simple geometry is acceptable
- Raw and cooked visuals should be visually distinguishable

Example prefab structure:

```text
Meat
├── IngredientInstance
├── IngredientVisual
├── RawVisual
└── PreparedVisual
```

Cooking changes the runtime ingredient state from:

```text
Raw → Prepared
```

and the visual switches accordingly.

---

# 6. Ingredient Prefab Architecture

Each physical ingredient is represented by an `IngredientInstance`.

```text
IngredientDefinition
        ↓
IngredientInstance
        ↓
IngredientVisual
        ↓
Raw / Prepared visual
```

`IngredientDefinition` is a ScriptableObject containing:

- Ingredient type
- Score value
- Whether preparation is required
- Preparation time

Current definitions:

```text
Vegetable
    Type: Vegetable
    Score: 20
    Preparation: Yes
    Time: 2 seconds

Cheese
    Type: Cheese
    Score: 10
    Preparation: No
    Time: 0 seconds

Meat
    Type: Meat
    Score: 30
    Preparation: Yes
    Time: 6 seconds
```

---

# 7. Player Inventory

The player can hold **only one ingredient at a time**.

```text
Player
├── CharacterController
├── PlayerController
├── PlayerInputHandler
└── PlayerInventory
    └── HandPoint
```

The ingredient is parented to `HandPoint` while being carried.

This prevents the player from carrying multiple ingredients simultaneously.

---

# 8. Preparation Stations

## Chopping Table

The chopping table accepts:

```text
Raw Vegetable
```

Preparation takes:

```text
2 seconds
```

Only one vegetable can be processed at the chopping table at a time.

The player can walk away while preparation continues.

After completion:

```text
Vegetable Raw
      ↓
  2 seconds
      ↓
Vegetable Prepared
```

---

## Stove

The stove accepts:

```text
Raw Meat
```

It contains **two cooking slots**.

Each slot processes its meat independently.

```text
Stove
├── Cooking Slot 1
└── Cooking Slot 2
```

Cooking takes:

```text
6 seconds
```

Cooking continues while the player walks away.

When finished, the meat becomes prepared and can be collected when the player's hand is empty.

---

## Trash

The trash allows the player to discard the ingredient currently being held.

This is important when:

- The player picked up the wrong ingredient.
- The player has no free hand.
- The player needs to clear the hand before collecting a prepared ingredient.

---

# 9. Orders

Orders are generated randomly.

Each order contains either:

- 2 ingredients, or
- 3 ingredients.

The probability is approximately:

```text
50% → 2 ingredients
50% → 3 ingredients
```

Ingredient selection is random, and duplicates are allowed.

Example:

```text
Vegetable + Cheese
```

or:

```text
Meat + Meat + Cheese
```

Each customer window owns one current order.

---

# 10. Customer Windows

There are four customer windows.

Each window:

- Displays the required ingredients.
- Displays how long the order has been open.
- Tracks delivered ingredients.
- Accepts only valid prepared ingredients.
- Completes when all required ingredients are delivered.

An invalid ingredient is not consumed and remains in the player's hand.

When an order is completed:

```text
Order Complete
      ↓
Score calculated
      ↓
Completion popup
      ↓
Order cleared
      ↓
5 second delay
      ↓
New order
```

---

# 11. Order UI

Each customer order has a dedicated UI card.

```text
OrderCard
├── Timer
├── Progress
├── Ingredient 1
├── Ingredient 2
└── Ingredient 3
```

A 2-ingredient order leaves the unused third slot empty.

## Normal Ingredient Images

The order UI uses separate sprites for:

```text
Vegetable
Cheese
Meat
```

## Delivered Ingredient Images

Delivered ingredients use separate artwork:

```text
Delivered Vegetable
Delivered Cheese
Delivered Meat
```

The UI swaps the normal ingredient image for the delivered image when that specific required ingredient has been delivered.

This means the order can visually progress:

```text
[Vegetable] [Cheese] [Meat]
```

to:

```text
[Delivered Vegetable] [Cheese] [Meat]
```

and eventually:

```text
[Delivered Vegetable] [Delivered Cheese] [Delivered Meat]
```

Duplicate ingredients are supported.

Example:

```text
[Meat] [Meat] [Cheese]
```

After one meat is delivered:

```text
[Delivered Meat] [Meat] [Cheese]
```

---


# 12A. Score Calculation — Detailed Examples

## Score Formula

The score for each completed order is calculated using:

```text
Order Score = Total Ingredient Value - floor(Seconds Open)
```

Where:

```text
Total Ingredient Value
= sum of the score values of every ingredient required by the order
```

Current ingredient values:

```text
Vegetable = 20
Cheese    = 10
Meat      = 30
```

`floor(Seconds Open)` means only the completed whole seconds are subtracted.

### Example 1 — Fast 2-Meat Order

Order:

```text
Meat + Meat
```

Ingredient value:

```text
30 + 30 = 60
```

Order completed after:

```text
0 seconds
```

Calculation:

```text
60 - floor(0)
= 60 - 0
= +60
```

Final order score:

```text
+60
```

---

## Example 2 — Meat + Cheese

Order:

```text
Meat + Cheese
```

Ingredient value:

```text
30 + 10 = 40
```

Completed after:

```text
1 second
```

Calculation:

```text
40 - floor(1)
= 40 - 1
= +39
```

Final order score:

```text
+39
```

---

## Example 3 — Vegetable + Cheese + Meat

Order:

```text
Vegetable + Cheese + Meat
```

Ingredient value:

```text
20 + 10 + 30 = 60
```

Completed after:

```text
5 seconds
```

Calculation:

```text
60 - floor(5)
= 60 - 5
= +55
```

Final order score:

```text
+55
```

---

## Example 4 — Slow Order With Positive Score

Order:

```text
Vegetable + Cheese
```

Ingredient value:

```text
20 + 10 = 30
```

Completed after:

```text
20 seconds
```

Calculation:

```text
30 - floor(20)
= 30 - 20
= +10
```

Final order score:

```text
+10
```

---

## Example 5 — Negative Order Score

An order can become negative when the order has been open longer than its ingredient value.

Order:

```text
Meat + Cheese
```

Ingredient value:

```text
30 + 10 = 40
```

Completed after:

```text
70 seconds
```

Calculation:

```text
40 - floor(70)
= 40 - 70
= -30
```

Final order score:

```text
-30
```

A negative order score is valid.

---

## Example 6 — Another Negative Score

Order:

```text
Vegetable + Cheese
```

Ingredient value:

```text
20 + 10 = 30
```

Completed after:

```text
45 seconds
```

Calculation:

```text
30 - floor(45)
= 30 - 45
= -15
```

Final order score:

```text
-15
```

---

# 12B. Cumulative Game Score

The player's main score is the sum of every completed order's score.

```text
Game Score = Order 1 Score + Order 2 Score + Order 3 Score + ...
```

Positive and negative order scores are both included.

### Example

First order:

```text
+60
```

Second order:

```text
-30
```

Third order:

```text
+25
```

Calculation:

```text
60 + (-30) + 25
= 60 - 30 + 25
= 55
```

Final game score:

```text
55
```

### Important

A negative order does **not** mean the entire score is permanently set to zero.

It is simply added as a negative value:

```text
Current Score + Negative Order Score
```

For example:

```text
60 + (-30) = 30
```

So:

```text
Before completing order: 60
Order score:             -30
After completing order:   30
```

---

# 12C. Score Calculation Reference Table

| Order | Ingredient Value | Seconds Open | Calculation | Order Score |
|---|---:|---:|---|---:|
| Meat + Meat | 60 | 0 | 60 - 0 | +60 |
| Meat + Cheese | 40 | 1 | 40 - 1 | +39 |
| Vegetable + Cheese + Meat | 60 | 5 | 60 - 5 | +55 |
| Vegetable + Cheese | 30 | 20 | 30 - 20 | +10 |
| Meat + Cheese | 40 | 70 | 40 - 70 | -30 |
| Vegetable + Cheese | 30 | 45 | 30 - 45 | -15 |

---

# 12D. Why Scores Can Look Different

Two orders can produce different scores because both of these values can change:

1. The ingredients in the randomly generated order.
2. How many seconds the order has been open.

For example:

```text
Order A:
Meat + Meat
30 + 30 = 60
Completed after 0 sec

Score = 60
```

while:

```text
Order B:
Meat + Cheese
30 + 10 = 40
Completed after 1 sec

Score = 39
```

Therefore, seeing:

```text
60
39
30
-30
```

does not automatically indicate a scoring bug.

The calculation should always be checked against:

```text
ingredient values
+
seconds the specific order was open
```

---

# 12E. Negative Score Threshold

An order reaches zero when:

```text
Total Ingredient Value - Seconds Open = 0
```

Therefore:

```text
Seconds Open = Total Ingredient Value
```

After that point, the order score becomes negative.

Example:

```text
Meat + Cheese
Ingredient Value = 40
```

At 40 seconds:

```text
40 - 40 = 0
```

At 41 seconds:

```text
40 - 41 = -1
```

At 70 seconds:

```text
40 - 70 = -30
```

This is why delaying an order can reduce the player's total score.

---

# 12F. High Score vs Current Score

The current score may be negative:

```text
Current Score = -30
```

The persistent high score should represent the best achieved score and should not be displayed as a negative value.

Example:

```text
Current Score: -30
High Score:     60
```

If a session finishes with:

```text
Final Score = 75
```

and the previous high score was:

```text
60
```

then:

```text
75 > 60
```

so the new high score becomes:

```text
75
```

If the final score is:

```text
40
```

while the high score is:

```text
60
```

then the high score remains:

```text
60
```


# 12. Scoring

The score for an order is:

```text
Sum of ingredient values - floor(seconds the order has been open)
```

Ingredient values:

```text
Vegetable = 20
Cheese    = 10
Meat      = 30
```

Example:

```text
Meat + Meat
30 + 30 = 60

Completed after 0 seconds:

60 - 0 = 60
```

Another example:

```text
Meat + Cheese
30 + 10 = 40

Completed after 1 second:

40 - 1 = 39
```

Scores can therefore become negative.

The overall score is the cumulative result of completed orders.

---

# 13. High Score

The high score is stored using Unity `PlayerPrefs`.

The high score persists between sessions.

The high score should not become negative.

Example:

```text
Current Score: -30
High Score: 60
```

A new high score is saved only when the final score exceeds the existing high score.

---

# 14. Session

Each game session lasts:

```text
3 minutes / 180 seconds
```

Session states:

```text
NotStarted
Playing
Paused
Finished
```

When the timer reaches zero:

```text
Playing
   ↓
Finished
```

The final score is evaluated against the persistent high score.

---

# 15. Input

Desktop controls:

```text
W A S D → Move
E       → Interact
ESC     → Pause
```

The project also contains mobile/touch input data in the Input Actions asset.

Mobile behavior is intentionally deferred until the desktop gameplay is fully tested.

---

# 16. Interaction Architecture

Interactions use a common interface:

```csharp
IInteractable
```

Implemented by:

```text
Refrigerator
ChoppingTable
Stove
CustomerWindowPoint
Trash
```

The player detects nearby interactables using:

```text
Physics.OverlapSphere
```

and an `Interactable` layer.

This allows different kitchen objects to share the same player interaction system while keeping their gameplay logic independent.

---

# 17. Main Architecture

```text
GameManager
├── GameSession
├── ScoreManager
├── HighScoreManager
└── GameTimer

PlayerController
├── PlayerInputHandler
└── PlayerInventory

IInteractable
├── Refrigerator
├── ChoppingTable
├── Stove
├── CustomerWindowPoint
└── Trash

Ingredients
├── IngredientDefinition
├── IngredientInstance
└── IngredientVisual

Orders
├── Order
├── OrderGenerator
├── OrderManager
├── CustomerWindow
└── CustomerWindowPoint

UI
├── ScoreUI
├── TimerUI
├── OrderUI
├── IngredientIconUI
├── InteractionPromptUI
├── ControlsUI
├── OrderCompletionUI
├── PauseMenuUI
└── SessionEndUI
```

---

# 18. Visual Direction

The game intentionally uses a **simple stylized 3D kitchen**.

The visuals prioritize:

1. Readability
2. Clear silhouettes
3. Fast recognition
4. Consistent scale
5. Simple materials
6. Clear prepared/raw state differences

High-detail realistic assets are not required.

The ingredient models should remain visually distinct from one another when viewed from the fixed perspective camera.

---

# 19. Current Development Status

### Gameplay

- [x] Player movement
- [x] Keyboard input
- [x] Refrigerator interaction
- [x] Ingredient spawning
- [x] One-item inventory
- [x] Vegetable preparation
- [x] Meat cooking
- [x] Two stove slots
- [x] Trash interaction
- [x] Customer windows
- [x] Random order generation
- [x] Order delivery
- [x] Order completion
- [x] Scoring
- [x] High score persistence
- [x] 3-minute timer

### UI

- [x] Score
- [x] High score
- [x] Session timer
- [x] Order cards
- [x] Ingredient icons
- [x] Delivered ingredient visuals
- [x] Completion popup
- [x] Controls panel
- [x] Pause panel

### Remaining

- [ ] Complete desktop playtest
- [ ] Fix remaining visual/gameplay bugs
- [ ] Final Inspector/reference audit
- [ ] Final UI polish
- [ ] Session-end screen verification
- [ ] Final submission/deliverable check

---

# 20. Design Principle

The project favors **small, focused systems with clear responsibilities**.

Gameplay logic should remain separate from:

- Visual presentation
- UI
- Order generation
- Session management
- Input handling

When fixing bugs, prefer the smallest targeted change rather than rewriting working systems.

The current priority is **desktop gameplay stability and visual bug fixing** before additional features or mobile adaptation.
