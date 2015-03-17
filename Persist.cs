using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public class Persist : MonoBehaviour
{
	static private Persist instance;

	void Awake()
	{
		instance = this;

		form = new WWWForm();
	}

	////////////////////////////////////// PLAYER PREFS WRAPPER ///////////////////////////////////
	///////////////////////////////////////////////////////////////////////////////////////////////
	///
	/// <summary>
	/// Saves the prefs.
	/// </summary>
	public static void SavePrefs()
	{
		PlayerPrefs.Save(); // Write on disk
	}
	/// <summary>
	/// Determines if has key the specified key.
	/// </summary>
	/// <returns><c>true</c> if has key the specified key; otherwise, <c>false</c>.</returns>
	/// <param name="key">Key.</param>
	public static bool HasKey( string key )
	{
		return PlayerPrefs.HasKey( key );
	}
	/// <summary>
	/// Deletes the key.
	/// </summary>
	/// <param name="key">Key.</param>
	public static void DeleteKey( string key )
	{
		PlayerPrefs.DeleteKey( key );
	}
	/// <summary>
	/// Deletes all.
	/// </summary>
	public static void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
	}
	/*************** SETTERS ************************/
	/*********************************************************************/

	/// <summary>
	/// Sets the string.
	/// </summary>
	/// <param name="key">Key.</param>
	/// <param name="value">Value.</param>
	public static void SetString( string key, string value )
	{
		PlayerPrefs.SetString( key, value );
	}
	/// <summary>
	/// Sets the int.
	/// </summary>
	/// <param name="key">Key.</param>
	/// <param name="value">Value.</param>
	public static void SetInt( string key, int value )
	{
		PlayerPrefs.SetInt( key, value );
	}
	/// <summary>
	/// Sets the bool.
	/// </summary>
	/// <param name="key">Key.</param>
	/// <param name="value">If set to <c>true</c> value.</param>
	public static void SetBool( string key, bool value )
	{
		PlayerPrefs.SetInt( key, (value) ? 1 : 0 );
	}
	/// <summary>
	/// Sets the float.
	/// </summary>
	/// <param name="key">Key.</param>
	/// <param name="value">Value.</param>
	public static void SetFloat( string key, float value )
	{
		PlayerPrefs.SetFloat( key, value );
	}
	/// <summary>
	/// Sets the binary.
	/// </summary>
	/// <param name="key">Key.</param>
	/// <param name="data">Data.</param>
	public static void SetBinary( string key, object data )
	{
		BinaryFormatter bf = new BinaryFormatter();
		MemoryStream ms = new MemoryStream();
		bf.Serialize( ms, data );
		PlayerPrefs.SetString( key, Convert.ToBase64String( ms.GetBuffer() ) );
	}

	/*************** END SETTERS ************************/
	/*************************************************************************/


	/*************** GETTERS ************************/
	/*********************************************************************/

	/// <summary>
	/// Gets the string.
	/// </summary>
	/// <returns>The string.</returns>
	/// <param name="key">Key.</param>
	public static string GetString( string key, string defaultValue = "" )
	{
		return PlayerPrefs.GetString( key, defaultValue );
	}
	/// <summary>
	/// Gets the int.
	/// </summary>
	/// <returns>The int.</returns>
	/// <param name="key">Key.</param>
	public static int GetInt( string key, int defaultValue = 0 )
	{
		return PlayerPrefs.GetInt( key, defaultValue );
	}
	/// <summary>
	/// Gets the bool.
	/// </summary>
	/// <returns><c>true</c>, if bool was gotten, <c>false</c> otherwise.</returns>
	/// <param name="key">Key.</param>
	public static bool GetBool( string key, bool defaultValue = false )
	{
		return (PlayerPrefs.GetInt( key, (!defaultValue) ? 0 : 1 ) == 1) ? true : false;
	}
	/// <summary>
	/// Gets the float.
	/// </summary>
	/// <returns>The float.</returns>
	/// <param name="key">Key.</param>
	public static float GetFloat( string key, float defaultValue = 0f )
	{
		return PlayerPrefs.GetFloat( key, defaultValue );
	}
	/// <summary>
	/// Gets the binary.
	/// </summary>
	/// <returns>The binary.</returns>
	/// <param name="key">Key.</param>
	/// <param name="defaultValue">Default value.</param>
	public static object GetBinary( string key, string defaultValue = "" )
	{
		string strData = PlayerPrefs.GetString( key, defaultValue );
		if ( !string.IsNullOrEmpty( strData ) )
		{
			BinaryFormatter bf = new BinaryFormatter();
			MemoryStream ms = new MemoryStream( Convert.FromBase64String( strData ) );
			return bf.Deserialize( ms );
		}
		return null;
	}

	/*************** END GETTERS ************************/
	/*************************************************************************/
	///
	////////////////////////////////////// END PLAYER PREFS WRAPPER ///////////////////////////////
	///////////////////////////////////////////////////////////////////////////////////////////////


	////////////////////////////////////// BINARY FILE HANDLER ////////////////////////////////////
	///////////////////////////////////////////////////////////////////////////////////////////////
	///
	/// <summary>
	/// Write a file from a serialize object class on disk.
	/// Class must be [Serializable]
	/// </summary>
	/// <param name="data">object.</param>
	/// <param name="filename">Filename.</param>
	public static void SaveBinary( object data, string filename )
	{
		string dataPath = Application.persistentDataPath;
#if UNITY_WEBPLAYER
		dataPath = Application.dataPath; // Its the only path It exists in web enviroment
#endif

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create( dataPath + "/" + filename + ".bin" );
		bf.Serialize( file, data );
		file.Close();
	}
	/// <summary>
	/// Loads the binary.
	/// </summary>
	/// <returns>System.object needs be casted to wanted class.</returns>
	/// <param name="filename">Filename.</param>
	public static object LoadBinary( string filename )
	{
#if !UNITY_WEBPLAYER
		if ( File.Exists( Application.persistentDataPath + "/" + filename + ".bin" ) )
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open( Application.persistentDataPath + "/" + filename + ".bin", FileMode.Open );
			object data = bf.Deserialize( file );
			file.Close();
			return data;
		}
		return null;
#else
		TextAsset text = Resources.Load<TextAsset>( filename );
		if ( text != null )
		{
			BinaryFormatter bf = new BinaryFormatter();
			Stream s = new MemoryStream( text.bytes );
			object data = bf.Deserialize( s );
			return data;
		}
		else
		{
			return null;
		}
#endif
	}
	///
	////////////////////////////////////// END BINARY FILE HANDLER ////////////////////////////////
	///////////////////////////////////////////////////////////////////////////////////////////////


	////////////////////////////////////// WWW FILE HANDLER ////////////////////////////////////
	///////////////////////////////////////////////////////////////////////////////////////////////
	///
	private static WWWForm form;
	/// <summary>
	/// Prepare fields to send in form.
	/// </summary>
	/// <param name="key">Key.</param>
	/// <param name="value">Value.</param>
	public static void WWWAddField( string key, object value, string binFilename = "" )
	{
		switch ( value.GetType().ToString() )
		{
		case "System.Int32":
			form.AddField( key, (int) value );
			break;
		case "System.String":
			form.AddField( key, (string) value );
			break;
		case "System.byte[]":
			form.AddBinaryData( key, (byte[]) value, binFilename );
			break;
		default:
			break;
		}
	}
	/// <summary>
	/// WWWs the remove all fields.
	/// </summary>
	public static void WWWRemoveAllFields()
	{
		form = new WWWForm();
	}
	/// <summary>
	/// WWWs the save.
	/// </summary>
	/// <param name="urlWebService">URL web service.</param>
	public static void WWWSave( string urlWebService )
	{
		instance.StartCoroutine( "ProcessWWWSave", urlWebService );
	}
	/// <summary>
	/// Processes the WWW save.
	/// </summary>
	/// <returns>The WWW save.</returns>
	/// <param name="url">URL.</param>
	private static IEnumerator ProcessWWWSave( object url )
	{
		WWW post = new WWW( (string) url, form );

		yield return post;

		if ( post.error != "" )
		{
			// Handle error
			Debug.Log( post.error );
		}
		WWWRemoveAllFields();
	}
	///
	////////////////////////////////////// WWW FILE HANDLER ////////////////////////////////////
	///////////////////////////////////////////////////////////////////////////////////////////////
}
