using System.Collections.Generic;
using System.Text;
using UnityEngine.LowLevel;

namespace UnityUtils.LowLevel
{
    public class PlayerLoopUtils
    {
        public static void RemoveSystem<T>(ref PlayerLoopSystem loop,
            in PlayerLoopSystem loopSystemToRemove)
        {
            if (loop.subSystemList == null) return;
            var playerLoopSystemList = new List<PlayerLoopSystem>(loop.subSystemList);
            for (int i = 0; i < playerLoopSystemList.Count; i++)
            {
                if (playerLoopSystemList[i].type == loopSystemToRemove.type &&
                    playerLoopSystemList[i].updateDelegate == loopSystemToRemove.updateDelegate)
                {
                    playerLoopSystemList.RemoveAt(i);
                    loop.subSystemList = playerLoopSystemList.ToArray();
                    return;
                }
            }
        }
        static void HandleSubSystemLoopForRemoval<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem systemToRemove)
        {
            if (loop.subSystemList == null)
                return;
            for (int i = 0; i < loop.subSystemList.Length; i++)
            {
               RemoveSystem<T>(ref loop.subSystemList[i], in systemToRemove);
            }
        }
        
        public static bool InserSystem<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem systemToInsert, int index){
            if(loop.type !=typeof(T)) return HandleSubSystemLoop<T>(ref loop, systemToInsert, index);
            
            var playerLoopSystemList = new List<PlayerLoopSystem>();
            if(loop.subSystemList != null) playerLoopSystemList.AddRange(loop.subSystemList);
            playerLoopSystemList.Add(systemToInsert);
            loop.subSystemList = playerLoopSystemList.ToArray();
            return true;
        }

        static bool  HandleSubSystemLoop<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem systemToInsert, int index)
        {
            if (loop.subSystemList == null)
                return false;
            for (int i = 0; i < loop.subSystemList.Length; i++)
            {
                if (!InserSystem<T>(ref loop.subSystemList[i], in systemToInsert, index)) 
                    continue;
                return true;

            }

            return false;
        }

        public static void PrintPlayerLoop(PlayerLoopSystem playerLoop)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Unity Player Loop");
            foreach (PlayerLoopSystem subSystem in playerLoop.subSystemList)
            {
                PrintSubsystem(subSystem,sb,0);
            }
        }

        static void PrintSubsystem(PlayerLoopSystem system, StringBuilder sb, int level)
        {
            sb.Append(' ',level*2).AppendLine(system.type.ToString());
            if(system.subSystemList==null||system.subSystemList.Length==0)
            {
                return;
            }

            foreach (PlayerLoopSystem subSystem in system.subSystemList)
            {
                PrintSubsystem(subSystem, sb, level+1);
            }
        }
    }
}