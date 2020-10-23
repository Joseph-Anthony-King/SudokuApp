using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Extensions;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Structs;

namespace SudokuCollective.Domain.Models
{
    public class SudokuMatrix : ISudokuMatrix
    {
        #region Properties
        public int Id { get; set; }
        public IGame Game { get; set; }
        public int DifficultyId { get; set; }
        public IDifficulty Difficulty { get; set; }
        public virtual List<ISudokuCell> SudokuCells { get; set; }

        public List<List<ISudokuCell>> Columns
        {
            get
            {
                var result = new List<List<ISudokuCell>>() {
                    FirstColumn,
                    SecondColumn,
                    ThirdColumn,
                    FourthColumn,
                    FifthColumn,
                    SixthColumn,
                    SeventhColumn,
                    EighthColumn,
                    NinthColumn
                };

                return result;
            }
        }
        public List<List<ISudokuCell>> Regions
        {
            get
            {
                var result = new List<List<ISudokuCell>>() {
                    FirstRegion,
                    SecondRegion,
                    ThirdRegion,
                    FourthRegion,
                    FifthRegion,
                    SixthRegion,
                    SeventhRegion,
                    EighthRegion,
                    NinthRegion
                };

                return result;
            }
        }
        public List<List<ISudokuCell>> Rows
        {
            get
            {
                var result = new List<List<ISudokuCell>>() {
                    FirstRow,
                    SecondRow,
                    ThirdRow,
                    FourthRow,
                    FifthRow,
                    SixthRow,
                    SeventhRow,
                    EighthRow,
                    NinthRow
                };

                return result;
            }
        }

        public List<ISudokuCell> FirstColumn { get => SudokuCells.Where(column => column.Column == 1).ToList(); }
        public List<ISudokuCell> SecondColumn { get => SudokuCells.Where(column => column.Column == 2).ToList(); }
        public List<ISudokuCell> ThirdColumn { get => SudokuCells.Where(column => column.Column == 3).ToList(); }
        public List<ISudokuCell> FourthColumn { get => SudokuCells.Where(column => column.Column == 4).ToList(); }
        public List<ISudokuCell> FifthColumn { get => SudokuCells.Where(column => column.Column == 5).ToList(); }
        public List<ISudokuCell> SixthColumn { get => SudokuCells.Where(column => column.Column == 6).ToList(); }
        public List<ISudokuCell> SeventhColumn { get => SudokuCells.Where(column => column.Column == 7).ToList(); }
        public List<ISudokuCell> EighthColumn { get => SudokuCells.Where(column => column.Column == 8).ToList(); }
        public List<ISudokuCell> NinthColumn { get => SudokuCells.Where(column => column.Column == 9).ToList(); }

        public List<ISudokuCell> FirstRegion { get => SudokuCells.Where(region => region.Region == 1).ToList(); }
        public List<ISudokuCell> SecondRegion { get => SudokuCells.Where(region => region.Region == 2).ToList(); }
        public List<ISudokuCell> ThirdRegion { get => SudokuCells.Where(region => region.Region == 3).ToList(); }
        public List<ISudokuCell> FourthRegion { get => SudokuCells.Where(region => region.Region == 4).ToList(); }
        public List<ISudokuCell> FifthRegion { get => SudokuCells.Where(region => region.Region == 5).ToList(); }
        public List<ISudokuCell> SixthRegion { get => SudokuCells.Where(region => region.Region == 6).ToList(); }
        public List<ISudokuCell> SeventhRegion { get => SudokuCells.Where(region => region.Region == 7).ToList(); }
        public List<ISudokuCell> EighthRegion { get => SudokuCells.Where(region => region.Region == 8).ToList(); }
        public List<ISudokuCell> NinthRegion { get => SudokuCells.Where(region => region.Region == 9).ToList(); }

        public List<ISudokuCell> FirstRow { get => SudokuCells.Where(row => row.Row == 1).ToList(); }
        public List<ISudokuCell> SecondRow { get => SudokuCells.Where(row => row.Row == 2).ToList(); }
        public List<ISudokuCell> ThirdRow { get => SudokuCells.Where(row => row.Row == 3).ToList(); }
        public List<ISudokuCell> FourthRow { get => SudokuCells.Where(row => row.Row == 4).ToList(); }
        public List<ISudokuCell> FifthRow { get => SudokuCells.Where(row => row.Row == 5).ToList(); }
        public List<ISudokuCell> SixthRow { get => SudokuCells.Where(row => row.Row == 6).ToList(); }
        public List<ISudokuCell> SeventhRow { get => SudokuCells.Where(row => row.Row == 7).ToList(); }
        public List<ISudokuCell> EighthRow { get => SudokuCells.Where(row => row.Row == 8).ToList(); }
        public List<ISudokuCell> NinthRow { get => SudokuCells.Where(row => row.Row == 9).ToList(); }

        public List<int> FirstColumnValues { get => SudokuCells.Where(column => column.Column == 1).Select(i => i.Value).Distinct().ToList(); }
        public List<int> SecondColumnValues { get => SudokuCells.Where(column => column.Column == 2).Select(i => i.Value).Distinct().ToList(); }
        public List<int> ThirdColumnValues { get => SudokuCells.Where(column => column.Column == 3).Select(i => i.Value).Distinct().ToList(); }
        public List<int> FourthColumnValues { get => SudokuCells.Where(column => column.Column == 4).Select(i => i.Value).Distinct().ToList(); }
        public List<int> FifthColumnValues { get => SudokuCells.Where(column => column.Column == 5).Select(i => i.Value).Distinct().ToList(); }
        public List<int> SixthColumnValues { get => SudokuCells.Where(column => column.Column == 6).Select(i => i.Value).Distinct().ToList(); }
        public List<int> SeventhColumnValues { get => SudokuCells.Where(column => column.Column == 7).Select(i => i.Value).Distinct().ToList(); }
        public List<int> EighthColumnValues { get => SudokuCells.Where(column => column.Column == 8).Select(i => i.Value).Distinct().ToList(); }
        public List<int> NinthColumnValues { get => SudokuCells.Where(column => column.Column == 9).Select(i => i.Value).Distinct().ToList(); }

        public List<int> FirstRegionValues { get => SudokuCells.Where(region => region.Region == 1).Select(i => i.Value).Distinct().ToList(); }
        public List<int> SecondRegionValues { get => SudokuCells.Where(region => region.Region == 2).Select(i => i.Value).Distinct().ToList(); }
        public List<int> ThirdRegionValues { get => SudokuCells.Where(region => region.Region == 3).Select(i => i.Value).Distinct().ToList(); }
        public List<int> FourthRegionValues { get => SudokuCells.Where(region => region.Region == 4).Select(i => i.Value).Distinct().ToList(); }
        public List<int> FifthRegionValues { get => SudokuCells.Where(region => region.Region == 5).Select(i => i.Value).Distinct().ToList(); }
        public List<int> SixthRegionValues { get => SudokuCells.Where(region => region.Region == 6).Select(i => i.Value).Distinct().ToList(); }
        public List<int> SeventhRegionValues { get => SudokuCells.Where(region => region.Region == 7).Select(i => i.Value).Distinct().ToList(); }
        public List<int> EighthRegionValues { get => SudokuCells.Where(region => region.Region == 8).Select(i => i.Value).Distinct().ToList(); }
        public List<int> NinthRegionValues { get => SudokuCells.Where(region => region.Region == 9).Select(i => i.Value).Distinct().ToList(); }

        public List<int> FirstRowValues { get => SudokuCells.Where(row => row.Row == 1).Select(i => i.Value).Distinct().ToList(); }
        public List<int> SecondRowValues { get => SudokuCells.Where(row => row.Row == 2).Select(i => i.Value).Distinct().ToList(); }
        public List<int> ThirdRowValues { get => SudokuCells.Where(row => row.Row == 3).Select(i => i.Value).Distinct().ToList(); }
        public List<int> FourthRowValues { get => SudokuCells.Where(row => row.Row == 4).Select(i => i.Value).Distinct().ToList(); }
        public List<int> FifthRowValues { get => SudokuCells.Where(row => row.Row == 5).Select(i => i.Value).Distinct().ToList(); }
        public List<int> SixthRowValues { get => SudokuCells.Where(row => row.Row == 6).Select(i => i.Value).Distinct().ToList(); }
        public List<int> SeventhRowValues { get => SudokuCells.Where(row => row.Row == 7).Select(i => i.Value).Distinct().ToList(); }
        public List<int> EighthRowValues { get => SudokuCells.Where(row => row.Row == 8).Select(i => i.Value).Distinct().ToList(); }
        public List<int> NinthRowValues { get => SudokuCells.Where(row => row.Row == 9).Select(i => i.Value).Distinct().ToList(); }
        #endregion

        #region Constructors
        public SudokuMatrix(List<int> intList) : this()
        {
            for (var i = 0; i < SudokuCells.Count; i++)
            {
                SudokuCells[i].Value = intList[i];
            }
        }

        public SudokuMatrix(string values) : this()
        {
            var intList = new List<int>();

            foreach (var value in values)
            {
                var s = char.ToString(value);

                if (Int32.TryParse(s, out var number))
                {
                    intList.Add(number);

                }
                else
                {
                    intList.Add(0);
                }
            }

            for (var i = 0; i < SudokuCells.Count; i++)
            {
                SudokuCells[i].Value = intList[i];
            }
        }

        public SudokuMatrix()
        {
            Id = 0;

            var rowColumnDeliminators = new List<int>() {
                9, 18, 27, 36, 45, 54, 63, 72 };
            var firstRegionDeliminators = new List<int>() {
                1, 2, 3, 10, 11, 12, 19, 20, 21 };
            var secondRegionDeliminators = new List<int>() {
                4, 5, 6, 13, 14, 15, 22, 23, 24 };
            var thirdRegionDeliminators = new List<int>() {
                7, 8, 9, 16, 17, 18, 25, 26, 27 };
            var fourthRegionDeliminators = new List<int>() {
                28, 29, 30, 37, 38, 39, 46, 47, 48 };
            var fifthRegionDeliminators = new List<int>() {
                31, 32, 33, 40, 41, 42, 49, 50, 51 };
            var sixthRegionDeliminators = new List<int>() {
                34, 35, 36, 43, 44, 45, 52, 53, 54 };
            var seventhRegionDeliminators = new List<int>() {
                55, 56, 57, 64, 65, 66, 73, 74, 75 };
            var eighthRegionDeliminators = new List<int>() {
                58, 59, 60, 67, 68, 69, 76, 77, 78 };
            var ninthRegionDeliminators = new List<int>() {
                61, 62, 63, 70, 71, 72, 79, 80, 81 };

            var columnIndexer = 1;
            var regionIndexer = 1;
            var rowIndexer = 1;

            SudokuCells = new List<ISudokuCell>();

            for (var i = 1; i < 82; i++)
            {

                if (firstRegionDeliminators.Contains(i))
                {
                    regionIndexer = 1;
                }
                else if (secondRegionDeliminators.Contains(i))
                {
                    regionIndexer = 2;
                }
                else if (thirdRegionDeliminators.Contains(i))
                {
                    regionIndexer = 3;
                }
                else if (fourthRegionDeliminators.Contains(i))
                {
                    regionIndexer = 4;
                }
                else if (fifthRegionDeliminators.Contains(i))
                {
                    regionIndexer = 5;
                }
                else if (sixthRegionDeliminators.Contains(i))
                {
                    regionIndexer = 6;
                }
                else if (seventhRegionDeliminators.Contains(i))
                {
                    regionIndexer = 7;
                }
                else if (eighthRegionDeliminators.Contains(i))
                {
                    regionIndexer = 8;
                }
                else
                {
                    regionIndexer = 9;
                }

                SudokuCells.Add(
                    new SudokuCell(
                        i,
                        columnIndexer,
                        regionIndexer,
                        rowIndexer,
                        Id
                    )
                );

                SudokuCells[i - 1].SudokuCellUpdatedEvent += HandleSudokuCellUpdatedEvent;
                SudokuCells[i - 1].SudokuCellResetEvent += HandleSudokuCellResetEvent;

                columnIndexer++;

                if (rowColumnDeliminators.Contains(i))
                {
                    columnIndexer = 1;
                    rowIndexer++;
                }
            }
        }

        [JsonConstructor]
        public SudokuMatrix(int id, int difficultyId)
        {
            Id = id;
            DifficultyId = difficultyId;
        }
        #endregion

        #region Methods

        public bool IsValid()
        {
            if (FirstColumnValues.Count == 9 && SecondColumnValues.Count == 9
                && ThirdColumnValues.Count == 9 && FourthColumnValues.Count == 9
                && FifthColumnValues.Count == 9 && SixthColumnValues.Count == 9
                && SeventhColumnValues.Count == 9 && EighthColumnValues.Count == 9
                && NinthColumnValues.Count == 9 && FirstRegionValues.Count == 9
                && SecondRegionValues.Count == 9 && ThirdRegionValues.Count == 9
                && FourthRegionValues.Count == 9 && FifthRegionValues.Count == 9
                && SixthRegionValues.Count == 9 && SeventhRegionValues.Count == 9
                && EighthRegionValues.Count == 9 && NinthRegionValues.Count == 9
                && FirstRowValues.Count == 9 && SecondRowValues.Count == 9
                && ThirdRowValues.Count == 9 && FourthRowValues.Count == 9
                && FifthRowValues.Count == 9 && SixthRowValues.Count == 9
                && SeventhRowValues.Count == 9 && EighthRowValues.Count == 9
                && NinthRowValues.Count == 9)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool IsSolved()
        {
            var result = true;

            var solution = ToInt32List();
            var usersAnsweres = ToDisplayedValuesList();

            for (var i = 0; i < solution.Count; i++)
            {

                if (solution[i] != usersAnsweres[i])
                {

                    result = false;
                }
            }

            return result;
        }

        public void GenerateSolution()
        {
            do
            {
                ZeroOutSudokuCells();

                foreach (var ISudokuCell in SudokuCells)
                {
                    if (ISudokuCell.Value == 0 && ISudokuCell.AvailableValues.Count > 1)
                    {
                        Random random = new Random();

                        CoreExtensions.Shuffle(ISudokuCell.AvailableValues, random);

                        ISudokuCell.Value = ISudokuCell.AvailableValues[0];
                    }
                    else if (ISudokuCell.Value == 0 && ISudokuCell.AvailableValues.Count == 1)
                    {
                        ISudokuCell.Value = ISudokuCell.AvailableValues[0];
                    }

                    if (ISudokuCell.Value == 0)
                    {
                        break;
                    }
                }

            } while (!IsValid());
        }

        public List<int> ToInt32List()
        {
            List<int> result = new List<int>();

            foreach (var ISudokuCell in SudokuCells)
            {
                result.Add(ISudokuCell.Value);
            }

            return result;
        }

        public List<int> ToDisplayedValuesList()
        {
            List<int> result = new List<int>();

            foreach (var ISudokuCell in SudokuCells)
            {
                result.Add(ISudokuCell.DisplayValue);
            }

            return result;
        }

        public void SetDifficulty(IDifficulty difficulty)
        {
            foreach (var ISudokuCell in SudokuCells)
            {
                ISudokuCell.Obscured = true;
            }

            Difficulty = difficulty;
            DifficultyId = difficulty.Id;

            int index;

            if (Difficulty.DifficultyLevel == DifficultyLevel.EASY)
            {
                index = 35;
            }
            else if (Difficulty.DifficultyLevel == DifficultyLevel.MEDIUM)
            {
                index = 29;
            }
            else if (Difficulty.DifficultyLevel == DifficultyLevel.HARD)
            {
                index = 23;
            }
            else
            {
                index = 17;
            }

            if (Difficulty.DifficultyLevel != DifficultyLevel.TEST)
            {
                List<int> indexerList = new List<int>();

                for (var i = 0; i < SudokuCells.Count; i++)
                {
                    indexerList.Add(i);
                }

                Random random = new Random();
                CoreExtensions.Shuffle(indexerList, random);

                for (var i = 0; i < index; i++)
                {
                    SudokuCells[indexerList[i]].Obscured = false;
                }
            }
            else
            {
                foreach (var ISudokuCell in SudokuCells)
                {
                    ISudokuCell.Obscured = false;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            foreach (var ISudokuCell in SudokuCells)
            {
                result.Append(ISudokuCell);
            }

            return result.ToString();
        }

        private void ZeroOutSudokuCells()
        {
            foreach (var ISudokuCell in SudokuCells)
            {
                ISudokuCell.Value = 0;
            }
        }
        #endregion

        #region Event Handlers
        public void HandleSudokuCellUpdatedEvent(
            object sender,
            UpdateSudokuCellEventArgs e)
        {
            foreach (var sudokuCell in SudokuCells)
            {
                if (sudokuCell.Index != e.Index)
                {
                    if (sudokuCell.Column == e.Column)
                    {
                        sudokuCell.UpdateAvailableValues(e.Value);
                    }
                    else if (sudokuCell.Region == e.Region)
                    {
                        sudokuCell.UpdateAvailableValues(e.Value);
                    }
                    else if (sudokuCell.Row == e.Row)
                    {
                        sudokuCell.UpdateAvailableValues(e.Value);
                    }
                    else
                    {
                        // do nothing...
                    }
                }
            }
        }

        public void HandleSudokuCellResetEvent(
            object sender,
            ResetSudokuCellEventArgs e)
        {
            if (e.Values.Count == 0)
            {
                var tmp = new List<int>();

                foreach (var ISudokuCell in SudokuCells)
                {
                    if (ISudokuCell.Index != e.Index)
                    {
                        if (ISudokuCell.Column == e.Column)
                        {
                            ISudokuCell.ResetAvailableValues(e.Value);
                        }
                        else if (ISudokuCell.Region == e.Region)
                        {
                            ISudokuCell.ResetAvailableValues(e.Value);
                        }
                        else if (ISudokuCell.Row == e.Row)
                        {
                            ISudokuCell.ResetAvailableValues(e.Value);
                        }
                        else
                        {
                            // do nothing...
                        }
                    }
                }

                foreach (var sudokuCell in SudokuCells)
                {
                    var allNineValues = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

                    if (sudokuCell.Index != e.Index)
                    {
                        if (e.Index == 1)
                        {
                            tmp = allNineValues.Except(FirstColumnValues).Except(FirstRegionValues).Except(FirstRowValues).ToList();
                        }
                        else if (e.Index == 2)
                        {
                            tmp = allNineValues.Except(SecondColumnValues).Except(FirstRegionValues).Except(FirstRowValues).ToList();
                        }
                        else if (e.Index == 3)
                        {
                            tmp = allNineValues.Except(ThirdColumnValues).Except(FirstRegionValues).Except(FirstRowValues).ToList();
                        }
                        else if (e.Index == 4)
                        {
                            tmp = allNineValues.Except(FourthColumnValues).Except(SecondRegionValues).Except(FirstRowValues).ToList();
                        }
                        else if (e.Index == 5)
                        {
                            tmp = allNineValues.Except(FifthColumnValues).Except(SecondRegionValues).Except(FirstRowValues).ToList();
                        }
                        else if (e.Index == 6)
                        {
                            tmp = allNineValues.Except(SixthColumnValues).Except(SecondRegionValues).Except(FirstRowValues).ToList();
                        }
                        else if (e.Index == 7)
                        {
                            tmp = allNineValues.Except(SeventhColumnValues).Except(ThirdRegionValues).Except(FirstRowValues).ToList();
                        }
                        else if (e.Index == 8)
                        {
                            tmp = allNineValues.Except(EighthColumnValues).Except(ThirdRegionValues).Except(FirstRowValues).ToList();
                        }
                        else if (e.Index == 9)
                        {
                            tmp = allNineValues.Except(NinthColumnValues).Except(ThirdRegionValues).Except(FirstRowValues).ToList();
                        }
                        else if (e.Index == 10)
                        {
                            tmp = allNineValues.Except(FirstColumnValues).Except(FirstRegionValues).Except(SecondRowValues).ToList();
                        }
                        else if (e.Index == 11)
                        {
                            tmp = allNineValues.Except(SecondColumnValues).Except(FirstRegionValues).Except(SecondRowValues).ToList();
                        }
                        else if (e.Index == 12)
                        {
                            tmp = allNineValues.Except(ThirdColumnValues).Except(FirstRegionValues).Except(SecondRowValues).ToList();
                        }
                        else if (e.Index == 13)
                        {
                            tmp = allNineValues.Except(FourthColumnValues).Except(SecondRegionValues).Except(SecondRowValues).ToList();
                        }
                        else if (e.Index == 14)
                        {
                            tmp = allNineValues.Except(FifthColumnValues).Except(SecondRegionValues).Except(SecondRowValues).ToList();
                        }
                        else if (e.Index == 15)
                        {
                            tmp = allNineValues.Except(SixthColumnValues).Except(SecondRegionValues).Except(SecondRowValues).ToList();
                        }
                        else if (e.Index == 16)
                        {
                            tmp = allNineValues.Except(SeventhColumnValues).Except(ThirdRegionValues).Except(SecondRowValues).ToList();
                        }
                        else if (e.Index == 17)
                        {
                            tmp = allNineValues.Except(EighthColumnValues).Except(ThirdRegionValues).Except(SecondRowValues).ToList();
                        }
                        else if (e.Index == 18)
                        {
                            tmp = allNineValues.Except(NinthColumnValues).Except(ThirdRegionValues).Except(SecondRowValues).ToList();
                        }
                        else if (e.Index == 19)
                        {
                            tmp = allNineValues.Except(FirstColumnValues).Except(FirstRegionValues).Except(ThirdRowValues).ToList();
                        }
                        else if (e.Index == 20)
                        {
                            tmp = allNineValues.Except(SecondColumnValues).Except(FirstRegionValues).Except(ThirdRowValues).ToList();
                        }
                        else if (e.Index == 21)
                        {
                            tmp = allNineValues.Except(ThirdColumnValues).Except(FirstRegionValues).Except(ThirdRowValues).ToList();
                        }
                        else if (e.Index == 22)
                        {
                            tmp = allNineValues.Except(FourthColumnValues).Except(SecondRegionValues).Except(ThirdRowValues).ToList();
                        }
                        else if (e.Index == 23)
                        {
                            tmp = allNineValues.Except(FifthColumnValues).Except(SecondRegionValues).Except(ThirdRowValues).ToList();
                        }
                        else if (e.Index == 24)
                        {
                            tmp = allNineValues.Except(SixthColumnValues).Except(SecondRegionValues).Except(ThirdRowValues).ToList();
                        }
                        else if (e.Index == 25)
                        {
                            tmp = allNineValues.Except(SeventhColumnValues).Except(ThirdRegionValues).Except(ThirdRowValues).ToList();
                        }
                        else if (e.Index == 26)
                        {
                            tmp = allNineValues.Except(EighthColumnValues).Except(ThirdRegionValues).Except(ThirdRowValues).ToList();
                        }
                        else if (e.Index == 27)
                        {
                            tmp = allNineValues.Except(NinthColumnValues).Except(ThirdRegionValues).Except(ThirdRowValues).ToList();
                        }
                        else if (e.Index == 28)
                        {
                            tmp = allNineValues.Except(FirstColumnValues).Except(FourthRegionValues).Except(FourthRowValues).ToList();
                        }
                        else if (e.Index == 29)
                        {
                            tmp = allNineValues.Except(SecondColumnValues).Except(FourthRegionValues).Except(FourthRowValues).ToList();
                        }
                        else if (e.Index == 30)
                        {
                            tmp = allNineValues.Except(ThirdColumnValues).Except(FourthRegionValues).Except(FourthRowValues).ToList();
                        }
                        else if (e.Index == 31)
                        {
                            tmp = allNineValues.Except(FourthColumnValues).Except(FifthRegionValues).Except(FourthRowValues).ToList();
                        }
                        else if (e.Index == 32)
                        {
                            tmp = allNineValues.Except(FifthColumnValues).Except(FifthRegionValues).Except(FourthRowValues).ToList();
                        }
                        else if (e.Index == 33)
                        {
                            tmp = allNineValues.Except(SixthColumnValues).Except(FifthRegionValues).Except(FourthRowValues).ToList();
                        }
                        else if (e.Index == 34)
                        {
                            tmp = allNineValues.Except(SeventhColumnValues).Except(SixthRegionValues).Except(FourthRowValues).ToList();
                        }
                        else if (e.Index == 35)
                        {
                            tmp = allNineValues.Except(EighthColumnValues).Except(SixthRegionValues).Except(FourthRowValues).ToList();
                        }
                        else if (e.Index == 36)
                        {
                            tmp = allNineValues.Except(NinthColumnValues).Except(SixthRegionValues).Except(FourthRowValues).ToList();
                        }
                        else if (e.Index == 37)
                        {
                            tmp = allNineValues.Except(FirstColumnValues).Except(FourthRegionValues).Except(FifthRowValues).ToList();
                        }
                        else if (e.Index == 38)
                        {
                            tmp = allNineValues.Except(SecondColumnValues).Except(FourthRegionValues).Except(FifthRowValues).ToList();
                        }
                        else if (e.Index == 39)
                        {
                            tmp = allNineValues.Except(ThirdColumnValues).Except(FourthRegionValues).Except(FifthRowValues).ToList();
                        }
                        else if (e.Index == 40)
                        {
                            tmp = allNineValues.Except(FourthColumnValues).Except(FifthRegionValues).Except(FifthRowValues).ToList();
                        }
                        else if (e.Index == 41)
                        {
                            tmp = allNineValues.Except(FifthColumnValues).Except(FifthRegionValues).Except(FifthRowValues).ToList();
                        }
                        else if (e.Index == 42)
                        {
                            tmp = allNineValues.Except(SixthColumnValues).Except(FifthRegionValues).Except(FifthRowValues).ToList();
                        }
                        else if (e.Index == 43)
                        {
                            tmp = allNineValues.Except(SeventhColumnValues).Except(SixthRegionValues).Except(FifthRowValues).ToList();
                        }
                        else if (e.Index == 44)
                        {
                            tmp = allNineValues.Except(EighthColumnValues).Except(SixthRegionValues).Except(FifthRowValues).ToList();
                        }
                        else if (e.Index == 45)
                        {
                            tmp = allNineValues.Except(NinthColumnValues).Except(SixthRegionValues).Except(FifthRowValues).ToList();
                        }
                        else if (e.Index == 46)
                        {
                            tmp = allNineValues.Except(FirstColumnValues).Except(FourthRegionValues).Except(SixthRowValues).ToList();
                        }
                        else if (e.Index == 47)
                        {
                            tmp = allNineValues.Except(SecondColumnValues).Except(FourthRegionValues).Except(SixthRowValues).ToList();
                        }
                        else if (e.Index == 48)
                        {
                            tmp = allNineValues.Except(ThirdColumnValues).Except(FourthRegionValues).Except(SixthRowValues).ToList();
                        }
                        else if (e.Index == 49)
                        {
                            tmp = allNineValues.Except(FourthColumnValues).Except(FifthRegionValues).Except(SixthRowValues).ToList();
                        }
                        else if (e.Index == 50)
                        {
                            tmp = allNineValues.Except(FifthColumnValues).Except(FifthRegionValues).Except(SixthRowValues).ToList();
                        }
                        else if (e.Index == 51)
                        {
                            tmp = allNineValues.Except(SixthColumnValues).Except(FifthRegionValues).Except(SixthRowValues).ToList();
                        }
                        else if (e.Index == 52)
                        {
                            tmp = allNineValues.Except(SeventhColumnValues).Except(SixthRegionValues).Except(SixthRowValues).ToList();
                        }
                        else if (e.Index == 53)
                        {
                            tmp = allNineValues.Except(EighthColumnValues).Except(SixthRegionValues).Except(SixthRowValues).ToList();
                        }
                        else if (e.Index == 54)
                        {
                            tmp = allNineValues.Except(NinthColumnValues).Except(SixthRegionValues).Except(SixthRowValues).ToList();
                        }
                        else if (e.Index == 55)
                        {
                            tmp = allNineValues.Except(FirstColumnValues).Except(SeventhRegionValues).Except(SeventhRowValues).ToList();
                        }
                        else if (e.Index == 56)
                        {
                            tmp = allNineValues.Except(SecondColumnValues).Except(SeventhRegionValues).Except(SeventhRowValues).ToList();
                        }
                        else if (e.Index == 57)
                        {
                            tmp = allNineValues.Except(ThirdColumnValues).Except(SeventhRegionValues).Except(SeventhRowValues).ToList();
                        }
                        else if (e.Index == 58)
                        {
                            tmp = allNineValues.Except(FourthColumnValues).Except(EighthRegionValues).Except(SeventhRowValues).ToList();
                        }
                        else if (e.Index == 59)
                        {
                            tmp = allNineValues.Except(FifthColumnValues).Except(EighthRegionValues).Except(SeventhRowValues).ToList();
                        }
                        else if (e.Index == 60)
                        {
                            tmp = allNineValues.Except(SixthColumnValues).Except(EighthRegionValues).Except(SeventhRowValues).ToList();
                        }
                        else if (e.Index == 61)
                        {
                            tmp = allNineValues.Except(SeventhColumnValues).Except(NinthRegionValues).Except(SeventhRowValues).ToList();
                        }
                        else if (e.Index == 62)
                        {
                            tmp = allNineValues.Except(EighthColumnValues).Except(NinthRegionValues).Except(SeventhRowValues).ToList();
                        }
                        else if (e.Index == 63)
                        {
                            tmp = allNineValues.Except(NinthColumnValues).Except(NinthRegionValues).Except(SeventhRowValues).ToList();
                        }
                        else if (e.Index == 64)
                        {
                            tmp = allNineValues.Except(FirstColumnValues).Except(SeventhRegionValues).Except(EighthRowValues).ToList();
                        }
                        else if (e.Index == 65)
                        {
                            tmp = allNineValues.Except(SecondColumnValues).Except(SeventhRegionValues).Except(EighthRowValues).ToList();
                        }
                        else if (e.Index == 66)
                        {
                            tmp = allNineValues.Except(ThirdColumnValues).Except(SeventhRegionValues).Except(EighthRowValues).ToList();
                        }
                        else if (e.Index == 67)
                        {
                            tmp = allNineValues.Except(FourthColumnValues).Except(EighthRegionValues).Except(EighthRowValues).ToList();
                        }
                        else if (e.Index == 68)
                        {
                            tmp = allNineValues.Except(FifthColumnValues).Except(EighthRegionValues).Except(EighthRowValues).ToList();
                        }
                        else if (e.Index == 69)
                        {
                            tmp = allNineValues.Except(SixthColumnValues).Except(EighthRegionValues).Except(EighthRowValues).ToList();
                        }
                        else if (e.Index == 70)
                        {
                            tmp = allNineValues.Except(SeventhColumnValues).Except(NinthRegionValues).Except(EighthRowValues).ToList();
                        }
                        else if (e.Index == 71)
                        {
                            tmp = allNineValues.Except(EighthColumnValues).Except(NinthRegionValues).Except(EighthRowValues).ToList();
                        }
                        else if (e.Index == 72)
                        {
                            tmp = allNineValues.Except(NinthColumnValues).Except(NinthRegionValues).Except(EighthRowValues).ToList();
                        }
                        else if (e.Index == 73)
                        {
                            tmp = allNineValues.Except(FirstColumnValues).Except(SeventhRegionValues).Except(NinthRowValues).ToList();
                        }
                        else if (e.Index == 74)
                        {
                            tmp = allNineValues.Except(SecondColumnValues).Except(SeventhRegionValues).Except(NinthRowValues).ToList();
                        }
                        else if (e.Index == 75)
                        {
                            tmp = allNineValues.Except(ThirdColumnValues).Except(SeventhRegionValues).Except(NinthRowValues).ToList();
                        }
                        else if (e.Index == 76)
                        {
                            tmp = allNineValues.Except(FourthColumnValues).Except(EighthRegionValues).Except(NinthRowValues).ToList();
                        }
                        else if (e.Index == 77)
                        {
                            tmp = allNineValues.Except(FifthColumnValues).Except(EighthRegionValues).Except(NinthRowValues).ToList();
                        }
                        else if (e.Index == 78)
                        {
                            tmp = allNineValues.Except(SixthColumnValues).Except(EighthRegionValues).Except(NinthRowValues).ToList();
                        }
                        else if (e.Index == 79)
                        {
                            tmp = allNineValues.Except(SeventhColumnValues).Except(NinthRegionValues).Except(NinthRowValues).ToList();
                        }
                        else if (e.Index == 80)
                        {
                            tmp = allNineValues.Except(EighthColumnValues).Except(NinthRegionValues).Except(NinthRowValues).ToList();
                        }
                        else if (e.Index == 81)
                        {
                            tmp = allNineValues.Except(NinthColumnValues).Except(NinthRegionValues).Except(NinthRowValues).ToList();
                        }
                        else
                        {
                            // do nothing...
                        }
                    }
                }

                var result = tmp.Distinct().ToList();

                if (result.Contains(0))
                {
                    result.Remove(0);
                }

                result.Sort();

                SudokuCells[e.Index - 1].AvailableValues.AddRange(result);
            }
        }
        #endregion
    }
}
