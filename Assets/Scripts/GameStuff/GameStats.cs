using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameStats", menuName = "Match3/GameStats", order = 0)]
public class GameStats : ScriptableObject
{
    [HideInInspector]
    public float itemWidth, itemHeight;
    [HideInInspector]
    public Vector2 boardStartPos;

    public int InitialMoves;

    public int boardXSize, boardYSize;

    public bool isEating, matched;

    //[NonSerialized]
    //public float runtimeMoves;

    #region Scores and Moves balance editable from inspector
    public int matched3Scores;
    public int matched3Moves;
    public int matched4Scores;
    public int matched4Moves;
    public int matched5Scores;
    public int matched5Moves;
    public int matchedManyScores;
    public int matchedManyMoves;
    public int notMatchedClickScore;
    public int notMatchedClickMovesPenalty;
    #endregion

    //public void OnAfterDeserialize()
    //{
    //    runtimeMoves = InitialMoves;
    //}
    //public void OnBeforeSerialize() {}
}
