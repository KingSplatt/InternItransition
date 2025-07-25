# Dice Game - Task3

This is a generative dice game where the user competes against the computer using custom dice with face values defined by the user.

## Description

The program implements a dice game where:
- At least 3 dice are provided as command line arguments
- Each die has faces with values between 0 and 9
- The computer selects a die first
- The user selects a die from the remaining ones
- A cryptographic key is generated to ensure fair randomness
- A round is played and the winner is determined

## Requirements

- Java 8 or higher
- javac compiler
- Windows/Linux/macOS operating system

## Compilation

### Compile all files at once
```cmd
javac *.java
```

## Execution

```cmd
java MainGame <dice1> <dice2> <dice3> [dice4] [dice5] ...
```

### Dice format
Each die is defined as a list of comma-separated values, where each value represents a face of the die.

### Execution Examples

#### Example 1: Basic dice
```cmd
java MainGame 2,2,4,4,9,9 6,8,1,1,8,6 7,5,3,7,5,3
```
#### Example 2: 4 Dice with same values
```cmd
java MainGame 1,2,3,4,5,6 1,2,3,4,5,6 1,2,3,4,5,6 1,2,3,4,5,6
```

#### Example 3: 3 different dice 
```cmd
java MainGame 2,2,4,4,9,9 1,1,6,6,8,8 3,3,5,5,7,7
```

#### Error Examples

##### Invalid numbers (contains letter 'a')
```cmd
java MainGame 1,2,3,4,5,a 6,7,8,9,10,11 1,2,3,4,5,6
```
**Error**: "Invalid face value in argument: 1,2,3,4,5,a"

##### Invalid face values (numbers > 9)
```cmd
java MainGame 1,2,3,4,5 6,7,8,9,10,11 1,2,3,4,5,6
```
**Error**: "Dice faces must be between 0 and 9. Invalid dice: 6,7,8,9,10,11"

##### Insufficient dice count (only 2 dice)
```cmd
java MainGame 1,2,3,4,5,6 2,3,4,5,6,7
```
**Error**: "You need to give at least 3 dice arguments"

## Game Rules

1. **Dice input**: Provide at least 3 dice as arguments
2. **Valid values**: Each die face must have a value between 0 and 9
3. **Dice selection**: 
   - The computer chooses a die first
   - The user selects a die from the remaining ones
4. **Key generation**: An HMAC cryptographic key is generated to ensure randomness
5. **Game**: Both players roll their dice and the result is compared
6. **Result**: The winner of the round is displayed

## Common Errors

### Error: "You need to give at least 3 dice arguments"
- **Cause**: Less than 3 dice were provided
- **Solution**: Make sure to provide at least 3 dice as arguments

### Error: "Dice faces must be between 0 and 9"
- **Cause**: A face value outside the range 0-9 was provided
- **Solution**: All face values must be between 0 and 9

### Error: "Invalid face value in argument"
- **Cause**: A non-numeric value was provided for a face
- **Solution**: Make sure all values are integers
