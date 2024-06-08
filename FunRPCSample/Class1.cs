using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunPlusEssentials;
using FunPlusEssentials.CustomContent;
using FunPlusEssentials.Other;
using FunPlusEssentials.Patches;
using MelonLoader;
using UnhollowerBaseLib;
using UnityEngine;


[assembly: MelonInfo(typeof(FunRPCSample.Main), "FunRPCTest", "1.0", "Vantablack")]
[assembly: MelonGame("ZeoWorks", "Slendytubbies 3")]
namespace FunRPCSample
{
    public class Main : MelonMod { }

    [HarmonyLib.HarmonyPatch(typeof(RoomMultiplayerMenu), "Awake")] 
    public static class OnRoomSpawned
    {
        static void Postfix(RoomMultiplayerMenu __instance)
        {
            Helper.Room.AddComponent<HelloWorldRPC>(); // добавляем компонент к объекту комнаты при ее спавне.
        }
    }

    // RegisterTypeInIl2Cpp - регистрация MonoBehaviour компонента в Il2Cpp, без этого атрибута компонент нельзя будет навесить на GameObject.
    // UsingRPC - обязательный атрибут который помечает что данный класс содержит в себе RPC методы.
    [RegisterTypeInIl2Cpp, UsingRPC] 
    public class HelloWorldRPC : MonoBehaviour
    {
        public HelloWorldRPC(IntPtr ptr) : base(ptr) { }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.U)) 
            {
                //отправляем RPC "Message" с параметром "HelloWorld" всем игрокам в комнате.
                Helper.Room.GetComponent<PhotonView>().RPC("Message", PhotonTargets.All, new Il2CppReferenceArray<Il2CppSystem.Object>(new Il2CppSystem.Object[] { "HelloWorld" }));
            }
        }

        [FunRPC] // аналогия [PunRPC]
        public void Message(string text)
        {
            CuteLogger.Meow(text); // выводим в консоль текст.
        }
    }
}
