using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    /// <summary>
    /// 计算对应点的三个方向
    /// 0：上前左[1，3，4]
    /// 1：上前右[0，2，5]
    /// 2：上后右[1，3，6]
    /// 3：上后左[0，2，7]
    /// 4：下前左[0，5，7]
    /// 5：下前右[1，4，6]
    /// 6：下后右[2，5，7]
    /// 7：下后左[3，4，6]
    /// </summary>
    /// <param name="index">需要的点的index</param>
    /// <param name="points">原bounds八个点数组[按照“上前左、上前右、上后右、上后左、下前左、下前右、下后右、下后左”顺序排列]</param>
    /// <returns></returns>
    public static Vector3[] CaculateBoundPointDir(int index, Vector3[] points)
    {
        Vector3[] dirs = new Vector3[3] { Vector3.zero, Vector3.zero, Vector3.zero };
        switch (index)
        {
            case 0:
                dirs[0] = (points[1] - points[0]).normalized;
                dirs[1] = (points[3] - points[0]).normalized;
                dirs[2] = (points[4] - points[0]).normalized;
                break;
            case 1:
                dirs[0] = (points[0] - points[1]).normalized;
                dirs[1] = (points[2] - points[1]).normalized;
                dirs[2] = (points[5] - points[1]).normalized;
                break;
            case 2:
                dirs[0] = (points[1] - points[2]).normalized;
                dirs[1] = (points[3] - points[2]).normalized;
                dirs[2] = (points[6] - points[2]).normalized;
                break;
            case 3:
                dirs[0] = (points[0] - points[3]).normalized;
                dirs[1] = (points[2] - points[3]).normalized;
                dirs[2] = (points[7] - points[3]).normalized;
                break;
            case 4:
                dirs[0] = (points[0] - points[4]).normalized;
                dirs[1] = (points[5] - points[4]).normalized;
                dirs[2] = (points[7] - points[4]).normalized;
                break;
            case 5:
                dirs[0] = (points[1] - points[5]).normalized;
                dirs[1] = (points[6] - points[5]).normalized;
                dirs[2] = (points[4] - points[5]).normalized;
                break;
            case 6:
                dirs[0] = (points[2] - points[6]).normalized;
                dirs[1] = (points[5] - points[6]).normalized;
                dirs[2] = (points[7] - points[6]).normalized;
                break;
            case 7:
                dirs[0] = (points[6] - points[7]).normalized;
                dirs[1] = (points[3] - points[7]).normalized;
                dirs[2] = (points[4] - points[7]).normalized;
                break;
        }
        return dirs;
    }
    
}
