﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SudokuApp.Models.Interfaces;

namespace SudokuApp.Models {

    public class SudokuCell : ISudokuCell {

        private int _value;
        private int _displayValue;
        private bool _initializing = true;

        #region Properties
        public int Id { get; set; }
        public SudokuMatrix SudokuMatrix { get; set; }
        public int Index { get; set; }
        public int Column { get; set; }
        public int Region { get; set; }
        public int Row { get; set; }
        public int Value {

            get => _value;
            set {

                if (value == 0) {

                    if (!_initializing) {

                        OnSuccessfulSudokuCellReset(
                            new ResetSudokuCellEventArgs(
                                Index,
                                _value,
                                Column,
                                Region,
                                Row
                            )
                        );

                        _value = value;
                    }

                } else {

                    AvailableValues = new List<int>();
                    _value = value;

                    OnSuccessfulSudokuCellUpdate(
                        new UpdateSudokuCellEventArgs(
                            Index,
                            _value,
                            Column,
                            Region,
                            Row
                        )
                    );
                }
            }
        }
        public int DisplayValue {

            get {
                
                if (!Obscured) {

                    return _value;

                } else {

                    return _displayValue;
                }
            }
            set {

                _displayValue = value;
            }
        }
        public bool Obscured { get; set; }
        [JsonIgnore]
        public List<int> AvailableValues { get; set; }
        #endregion

        #region Constructors
        public SudokuCell(int index, int column, int region, int row) {

            AvailableValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            Index = index;
            Column = column;
            Region = region;
            Row = row;

            Value = 0;
            Obscured = true;

            _initializing = false;
        }

        public SudokuCell(int index, int column, int region, int row, int value) {

            if (Value == 0) {

                AvailableValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                
            } else {

                AvailableValues = new List<int>();
            }

            Index = index;
            Column = column;
            Region = region;
            Row = row;

            Value = value;
            Obscured = true;

            _initializing = false;
        }
        #endregion

        public int ToInt32() => DisplayValue;

        public override string ToString() => DisplayValue.ToString();

        public void UpdateAvailableValues(int i) {

            if (i == 0) {

                AvailableValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            } else {

                if (AvailableValues.Contains(i) && AvailableValues.Count > 0) {

                    AvailableValues.Remove(i);
                }

                if (AvailableValues.Count == 1) {

                    Value = AvailableValues[0];
                }
            }
        }

        public void ResetAvailableValues(int i) {

            if (Value == 0 && !AvailableValues.Contains(i)) {

                AvailableValues.Add(i);
                var tmp = AvailableValues.Distinct().ToList();
                tmp.Remove(0);
                tmp.Sort();

                AvailableValues = new List<int>();
                AvailableValues.AddRange(tmp);
            }
        }

        public event EventHandler<UpdateSudokuCellEventArgs> SudokuCellUpdatedEvent;

        public event EventHandler<ResetSudokuCellEventArgs> SudokuCellResetEvent;

        public virtual void OnSuccessfulSudokuCellUpdate(UpdateSudokuCellEventArgs e) {

            SudokuCellUpdatedEvent.Invoke(this, e);
        }

        public virtual void OnSuccessfulSudokuCellReset(ResetSudokuCellEventArgs e) {

            SudokuCellResetEvent.Invoke(this, e);
        }
    }

    public struct UpdateSudokuCellEventArgs {

        public int Index { get; set; }
        public int Value { get; set; }
        public int Column { get; set; }
        public int Region { get; set; }
        public int Row { get; set; }

        public UpdateSudokuCellEventArgs(int index, int value, int column, int region, int row) {

            Index = index;
            Value = value;
            Column = column;
            Region = region;
            Row = row;
        }
    }

    public struct ResetSudokuCellEventArgs {

        public int Index { get; set; }
        public int Value { get; set; }
        public int Column { get; set; }
        public int Region { get; set; }
        public int Row { get; set; }
        public List<int> Values { get; set; }

        public ResetSudokuCellEventArgs(int index, int value, int column, int region, int row) {

            Index = index;
            Value = value;
            Column = column;
            Region = region;
            Row = row;
            Values = new List<int>();
        }
    }
}
