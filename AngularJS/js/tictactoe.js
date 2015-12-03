$(document).ready(function(){
  var board = [["","",""],["","",""],["","",""]];
  var turn = 0;
  var winner = 0;
  
  display_turn(turn);
  
  $('td').click(function(){
    var col = $(this).attr("data");
    var row = $(this).parent().attr("data");
    if ($(this).html() === "" && !winner){
      if (turn%2){
        $(this).html("X");
        board[row][col] = "X";
      }else{
        $(this).html("O");
        board[row][col] = "O";
      }
      turn++;
      display_turn(turn);
      
      winner = game_over(col, row, $(this).html());
      if (winner){
        $('#results').html("Game Over!!!<br />Player "+winner+" wins");
      }
    }
  });
  
  function game_over (col, row, player) {
    var result;
    var winner = (player == "O") ? 1 : 2;
    console.log(row+col+player);
    // vertically
    for (var dir = 0; dir < 3; dir++){
      switch (dir){
          // VERTICAL
        case 0:
          result = true;
          for (var i = 0; i < 3; i++) {
            if (board[i][col] != player)
              result = false;
          }
          break;
          // HORIZONTAL
        case 1:
          result = true;
          for (i = 0; i < 3; i++) {
            if (board[row][i] != player)
              result = false;
          }
          break;
          // DIAG
        case 2:
          result = true;
          for (i = 0; i < 3; i++) {
            if (board[i][i] != player)
              result = false;
          }
          if (result) return winner;
          result = true;
          var rev_index = 2;
          for (i = 0; i < 3; i++) {
            if (board[i][rev_index--] != player)
              return false;
          }
          break;
      }
      if (result)	return winner;
    }
  }
  
  function display_turn (n) {
    if (n%2){
      $('#results').html("Player 2's turn");
    }else{
      $('#results').html("Player 1's turn");
    }
  }
  
});