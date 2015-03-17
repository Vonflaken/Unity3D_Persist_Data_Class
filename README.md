# Unity3D_Persist_Data_Class
One file to wrap the save/load data functionalities of Unity3D, It makes easy to save/load any kind of data.

## Features
* It wraps util functions of PlayerPrefs:
  - Save.
  - HasKey.
  - DeleteKey.
  - DeleteAll.
* Getters/Setters of PlayerPrefs:
  - Integer.
  - String.
  - Float.
  - Bool.
  - **Binary**.
* Getter/Setter of **binary** files using OS FileSystem.
* It sends String, Integer or Byte[] data to be handled by some web script in remote server.    

**Disclaimer**: tested in Unity3D version 4. Net functions untested.

## Thanks
* Some article in unitygems.com was inspirational for this script.
