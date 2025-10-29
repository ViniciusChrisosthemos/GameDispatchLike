using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNotFoundException : Exception {}

public class BadFormatGameException : Exception { public BadFormatGameException(string message) : base(message) { } }
