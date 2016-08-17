using System;
using System.Collections.Generic;
using System.IO;

class Solution {
    static void next_move(int posr, int posc, String [] board){
        String path = "cells.txt";
        List<Cell> dirtyCells = new List<Cell>();
        List<Cell> unknownCells = new List<Cell>();
        
        //Get saved cells from file
        String savedCells;
        try {
            savedCells = File.ReadAllText(path);
        }
        catch(Exception e) {
            savedCells = "";
        }
        
        String cellsToSave = "";

        //Go through board and add cells and their value
        for (int i = 0; i < board.Length; i++) {

            for (int j = 0; j < board[0].Length; j++) {
                
                Char currentCellValue;
                
                //Get value of this cell position. If cell is unknown now, save previous value if it exists
                if (board[i][j].Equals('o')) {
                    try {
                        currentCellValue = savedCells[((i*5)+j)];
                    }
                    catch(Exception e) {
                        currentCellValue = board[i][j];
                    }
                }
                //Otherwise save current value
                else {
                    currentCellValue = board[i][j];
                }
                
                Cell newCell = new Cell(i, j, posr, posc);
                
                // Add dirty and unknown cells to list, and save cell values
                switch(currentCellValue) {
                    case 'o':
                        unknownCells.Add(newCell);
                        cellsToSave += currentCellValue;
                        break;
                    case 'b':
                    case '-':
                        cellsToSave += currentCellValue;
                        break;
                    case 'd':
                        dirtyCells.Add(newCell);
                        cellsToSave += currentCellValue;
                        break;
                }
            }
        }
        
        // Save currently known cell layout to file
        File.WriteAllText(path, cellsToSave);
        
        
        Cell closest;
        
        // Find closest dirty cell if there are any
        if (dirtyCells.Count > 0) {
            
            closest = dirtyCells[0];

            for (int i = 1; i < dirtyCells.Count; i++) {
                if (dirtyCells[i].getDistance() < closest.getDistance()) {
                    closest = dirtyCells[i];
                }
            }
        }
        
        // If no cells are known to be dirty, find closest unknown
        else {
            closest = unknownCells[0];
            
            for (int i = 1; i < unknownCells.Count; i++) {
                if (unknownCells[i].getDistance() < closest.getDistance()) {
                    closest = unknownCells[i];
                }
            }
        }
        
        // Movement logic. If on a dirty cell, clean it; otherwise move closer to selected cell
        if (closest.getDistance() == 0) {
                ut("CLEAN");
        }
        else {
            int closeR = closest.getR();
            int closeC = closest.getC();

            if (posc < closeC) {
                ut("RIGHT");
            }

            else if (posc > closeC) {
                ut("LEFT");
            }

            else if (posr < closeR) {
                ut("DOWN");
            }
            else if (posr > closeR) {
                ut("UP");
            }
        }
    }
    
    class Cell {
        private int row;
        private int column;
        private int distance = -1;

        public Cell(int r, int c, int posr, int posc) {
            row = r;
            column = c;
           
            findDistance(posr, posc);
        }
        

        public int getR() {
            return row;
        }

        public int getC() {
            return column;
        }

        private void findDistance(int posr, int posc) {
            distance = Math.Abs(posr - row) + Math.Abs(posc - column);
        }

        public int getDistance() {
            return distance;
        }
        
    }
    
    static void ut(String s) {
        Console.WriteLine(s);
    }
    
static void Main(String[] args) {
        String temp = Console.ReadLine();
        String[] position = temp.Split(' ');
        int[] pos = new int[2];
        String[] board = new String[5];
        for(int i=0;i<5;i++) {
            board[i] = Console.ReadLine();
        }
        for(int i=0;i<2;i++) pos[i] = Convert.ToInt32(position[i]);
        next_move(pos[0], pos[1], board);
    }
}
