using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuApp.Models {

    public class SudokuMatrix {

        public int Id { get; set; }

        public Difficulty Difficulty;

        public List<SudokuCell> SudokuCells;

        public List<List<SudokuCell>> Columns {

            get {

                var result = new List<List<SudokuCell>>() {
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

        public List<List<SudokuCell>> Regions {

            get {

                var result = new List<List<SudokuCell>>() {
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

        public List<List<SudokuCell>> Rows {

            get {

                var result = new List<List<SudokuCell>>() {
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

        #region Private Properties
        private List<SudokuCell> FirstColumn { get => SudokuCells.Where(column => column.Column == 1).ToList(); }
        private List<SudokuCell> SecondColumn { get => SudokuCells.Where(column => column.Column == 2).ToList(); }
        private List<SudokuCell> ThirdColumn { get => SudokuCells.Where(column => column.Column == 3).ToList(); }
        private List<SudokuCell> FourthColumn { get => SudokuCells.Where(column => column.Column == 4).ToList(); }
        private List<SudokuCell> FifthColumn { get => SudokuCells.Where(column => column.Column == 5).ToList(); }
        private List<SudokuCell> SixthColumn { get => SudokuCells.Where(column => column.Column == 6).ToList(); }
        private List<SudokuCell> SeventhColumn { get => SudokuCells.Where(column => column.Column == 7).ToList(); }
        private List<SudokuCell> EighthColumn { get => SudokuCells.Where(column => column.Column == 8).ToList(); }
        private List<SudokuCell> NinthColumn { get => SudokuCells.Where(column => column.Column == 9).ToList(); }

        private List<SudokuCell> FirstRegion { get => SudokuCells.Where(region => region.Region == 1).ToList(); }
        private List<SudokuCell> SecondRegion { get => SudokuCells.Where(region => region.Region == 2).ToList(); }
        private List<SudokuCell> ThirdRegion { get => SudokuCells.Where(region => region.Region == 3).ToList(); }
        private List<SudokuCell> FourthRegion { get => SudokuCells.Where(region => region.Region == 4).ToList(); }
        private List<SudokuCell> FifthRegion { get => SudokuCells.Where(region => region.Region == 5).ToList(); }
        private List<SudokuCell> SixthRegion { get => SudokuCells.Where(region => region.Region == 6).ToList(); }
        private List<SudokuCell> SeventhRegion { get => SudokuCells.Where(region => region.Region == 7).ToList(); }
        private List<SudokuCell> EighthRegion { get => SudokuCells.Where(region => region.Region == 8).ToList(); }
        private List<SudokuCell> NinthRegion { get => SudokuCells.Where(region => region.Region == 9).ToList(); }

        private List<SudokuCell> FirstRow { get => SudokuCells.Where(row => row.Row == 1).ToList(); }
        private List<SudokuCell> SecondRow { get => SudokuCells.Where(row => row.Row == 2).ToList(); }
        private List<SudokuCell> ThirdRow { get => SudokuCells.Where(row => row.Row == 3).ToList(); }
        private List<SudokuCell> FourthRow { get => SudokuCells.Where(row => row.Row == 4).ToList(); }
        private List<SudokuCell> FifthRow { get => SudokuCells.Where(row => row.Row == 5).ToList(); }
        private List<SudokuCell> SixthRow { get => SudokuCells.Where(row => row.Row == 6).ToList(); }
        private List<SudokuCell> SeventhRow { get => SudokuCells.Where(row => row.Row == 7).ToList(); }
        private List<SudokuCell> EighthRow { get => SudokuCells.Where(row => row.Row == 8).ToList(); }
        private List<SudokuCell> NinthRow { get => SudokuCells.Where(row => row.Row == 9).ToList(); }

        private List<int> FirstColumnValues { get => SudokuCells.Where(column => column.Column == 1).Select(i => i.Value).Distinct().ToList(); }
        private List<int> SecondColumnValues { get => SudokuCells.Where(column => column.Column == 2).Select(i => i.Value).Distinct().ToList(); }
        private List<int> ThirdColumnValues { get => SudokuCells.Where(column => column.Column == 3).Select(i => i.Value).Distinct().ToList(); }
        private List<int> FourthColumnValues { get => SudokuCells.Where(column => column.Column == 4).Select(i => i.Value).Distinct().ToList(); }
        private List<int> FifthColumnValues { get => SudokuCells.Where(column => column.Column == 5).Select(i => i.Value).Distinct().ToList(); }
        private List<int> SixthColumnValues { get => SudokuCells.Where(column => column.Column == 6).Select(i => i.Value).Distinct().ToList(); }
        private List<int> SeventhColumnValues { get => SudokuCells.Where(column => column.Column == 7).Select(i => i.Value).Distinct().ToList(); }
        private List<int> EighthColumnValues { get => SudokuCells.Where(column => column.Column == 8).Select(i => i.Value).Distinct().ToList(); }
        private List<int> NinthColumnValues { get => SudokuCells.Where(column => column.Column == 9).Select(i => i.Value).Distinct().ToList(); }

        private List<int> FirstRegionValues { get => SudokuCells.Where(region => region.Region == 1).Select(i => i.Value).Distinct().ToList(); }
        private List<int> SecondRegionValues { get => SudokuCells.Where(region => region.Region == 2).Select(i => i.Value).Distinct().ToList(); }
        private List<int> ThirdRegionValues { get => SudokuCells.Where(region => region.Region == 3).Select(i => i.Value).Distinct().ToList(); }
        private List<int> FourthRegionValues { get => SudokuCells.Where(region => region.Region == 4).Select(i => i.Value).Distinct().ToList(); }
        private List<int> FifthRegionValues { get => SudokuCells.Where(region => region.Region == 5).Select(i => i.Value).Distinct().ToList(); }
        private List<int> SixthRegionValues { get => SudokuCells.Where(region => region.Region == 6).Select(i => i.Value).Distinct().ToList(); }
        private List<int> SeventhRegionValues { get => SudokuCells.Where(region => region.Region == 7).Select(i => i.Value).Distinct().ToList(); }
        private List<int> EighthRegionValues { get => SudokuCells.Where(region => region.Region == 8).Select(i => i.Value).Distinct().ToList(); }
        private List<int> NinthRegionValues { get => SudokuCells.Where(region => region.Region == 9).Select(i => i.Value).Distinct().ToList(); }

        private List<int> FirstRowValues { get => SudokuCells.Where(row => row.Row == 1).Select(i => i.Value).Distinct().ToList(); }
        private List<int> SecondRowValues { get => SudokuCells.Where(row => row.Row == 2).Select(i => i.Value).Distinct().ToList(); }
        private List<int> ThirdRowValues { get => SudokuCells.Where(row => row.Row == 3).Select(i => i.Value).Distinct().ToList(); }
        private List<int> FourthRowValues { get => SudokuCells.Where(row => row.Row == 4).Select(i => i.Value).Distinct().ToList(); }
        private List<int> FifthRowValues { get => SudokuCells.Where(row => row.Row == 5).Select(i => i.Value).Distinct().ToList(); }
        private List<int> SixthRowValues { get => SudokuCells.Where(row => row.Row == 6).Select(i => i.Value).Distinct().ToList(); }
        private List<int> SeventhRowValues { get => SudokuCells.Where(row => row.Row == 7).Select(i => i.Value).Distinct().ToList(); }
        private List<int> EighthRowValues { get => SudokuCells.Where(row => row.Row == 8).Select(i => i.Value).Distinct().ToList(); }
        private List<int> NinthRowValues { get => SudokuCells.Where(row => row.Row == 9).Select(i => i.Value).Distinct().ToList(); }
        #endregion

        #region Constructors
        public SudokuMatrix() {

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

            SudokuCells = new List<SudokuCell>();

            for (var i = 1; i < 82; i++) {

                if (firstRegionDeliminators.Contains(i)) {

                    regionIndexer = 1;

                } else if (secondRegionDeliminators.Contains(i)) {

                    regionIndexer = 2;

                } else if (thirdRegionDeliminators.Contains(i)) {

                    regionIndexer = 3;

                } else if (fourthRegionDeliminators.Contains(i)) {

                    regionIndexer = 4;

                } else if (fifthRegionDeliminators.Contains(i)) {

                    regionIndexer = 5;

                } else if (sixthRegionDeliminators.Contains(i)) {

                    regionIndexer = 6;

                } else if (seventhRegionDeliminators.Contains(i)) {

                    regionIndexer = 7;

                } else if (eighthRegionDeliminators.Contains(i)) {

                    regionIndexer = 8;

                } else {

                    regionIndexer = 9;
                }

                SudokuCells.Add(
                    new SudokuCell(
                        i,
                        columnIndexer,
                        regionIndexer,
                        rowIndexer
                    )
                );

                SudokuCells[i - 1].SudokuCellUpdatedEvent += HandleSudokuCellUpdatedEvent;

                columnIndexer++;

                if (rowColumnDeliminators.Contains(i)) {

                    columnIndexer = 1;
                    rowIndexer++;
                }
            }
        }

        public SudokuMatrix(List<int> intList) : this() {

            for (var i = 0; i < SudokuCells.Count; i++) {

                SudokuCells[i].Value = intList[i];
            }
        }

        public SudokuMatrix(string values) : this() {

            var intList = new List<int>();

            foreach (var value in values) {

                var s = char.ToString(value);

                if (Int32.TryParse(s, out var number)) {

                    intList.Add(number);

                } else {

                    intList.Add(0);
                }
            }

            for (var i = 0; i < SudokuCells.Count; i++) {

                SudokuCells[i].Value = intList[i];
            }
        }
        #endregion

        public void GenerateSolution() {

            do {

                ZeroOutSudokuCells();

                foreach (var sudokuCell in SudokuCells) {

                    if (sudokuCell.AvailableValues.Count > 1 && sudokuCell.Value == 0) {

                        AppExtensions.AppExtensions.Shuffle(sudokuCell.AvailableValues);

                        sudokuCell.Value = sudokuCell.AvailableValues[0];

                        if (sudokuCell.Value == 0) {

                            break;
                        }
                    }
                }

            } while (!IsValid());
        }

        public List<int> ToInt32List() {

            List<int> result = new List<int>();

            foreach (var sudokuCell in SudokuCells) {

                result.Add(sudokuCell.Value);
            }

            return result;
        }

        public List<int> ToDisplayedValuesList() {

            List<int> result = new List<int>();

            foreach (var sudokuCell in SudokuCells) {

                result.Add(sudokuCell.DisplayValue);
            }

            return result;
        }

        public void SetDifficulty(Difficulty difficulty) {

            foreach (var sudokuCell in SudokuCells) {

                sudokuCell.Obscured = true;
            }

            this.Difficulty = difficulty;
            int index;

            if (this.Difficulty == Difficulty.EASY) {

                index = 35;

            } else if (this.Difficulty == Difficulty.MEDIUM) {

                index = 29;

            } else if (this.Difficulty == Difficulty.HARD) {

                index = 23;

            } else {

                index = 17;
            }

            if (this.Difficulty != Difficulty.TEST) {

                List<int> indexerList = new List<int>();

                for (var i = 0; i < SudokuCells.Count; i++) {

                    indexerList.Add(i);
                }

                AppExtensions.AppExtensions.Shuffle(indexerList);

                for (var i = 0; i < index; i++) {

                    SudokuCells[indexerList[i]].Obscured = false;
                }

            } else {

                foreach (var sudokuCell in SudokuCells) {

                    sudokuCell.Obscured = false;
                }
            }
        }

        public override string ToString() {

            StringBuilder result = new StringBuilder();

            foreach (var sudokuCell in SudokuCells) {

                result.Append(sudokuCell);
            }

            return result.ToString();
        }

        public bool IsValid() {

            if (FirstColumnValues.Count == 9 && SecondColumnValues.Count == 9 && ThirdColumnValues.Count == 9 && FourthColumnValues.Count == 9 && FifthColumnValues.Count == 9
                && SixthColumnValues.Count == 9 && SeventhColumnValues.Count == 9 && EighthColumnValues.Count == 9 && NinthColumnValues.Count == 9
                && FirstRegionValues.Count == 9 && SecondRegionValues.Count == 9 && ThirdRegionValues.Count == 9 && FourthRegionValues.Count == 9 && FifthRegionValues.Count == 9
                && SixthRegionValues.Count == 9 && SeventhRegionValues.Count == 9 && EighthRegionValues.Count == 9 && NinthRegionValues.Count == 9
                && FirstRowValues.Count == 9 && SecondRowValues.Count == 9 && ThirdRowValues.Count == 9 && FourthRowValues.Count == 9 && FifthRowValues.Count == 9
                && SixthRowValues.Count == 9 && SeventhRowValues.Count == 9 && EighthRowValues.Count == 9 && NinthRowValues.Count == 9) {

                return true;

            } else {

                return false;
            }
        }

        private void ZeroOutSudokuCells() {

            foreach (var sudokuCell in SudokuCells) {

                sudokuCell.Value = 0;
            }
        }

        internal void HandleSudokuCellUpdatedEvent(
            object sender,
            UpdateSudokuCellEventArgs e) {

            foreach (var sudokuCell in SudokuCells) {

                if (sudokuCell.Column == e.Column) {

                    sudokuCell.UpdateAvailableValues(e.Value);

                } else if (sudokuCell.Region == e.Region) {

                    sudokuCell.UpdateAvailableValues(e.Value);

                } else if (sudokuCell.Row == e.Row) {

                    sudokuCell.UpdateAvailableValues(e.Value);

                } else {

                    // do nothing...
                }
            }
        }
    }
}
