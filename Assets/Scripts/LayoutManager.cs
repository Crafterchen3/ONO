using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutManager : MonoBehaviour
{
    const float playerWidth = 4.6f;
    const float playerHeight = 2.8f;
    const float playerCorrection = 2.0f;
    const float padding = 0.5f;
    const float playingAreaWidth = 7.75f;
    const float playingAreaHeight = 3f;
    const float playingAreaUpperLeftX = -3.5f;
    const float playingAreaUpperLeftY = 1.5f;
    const float cardDisplayAreaHeight = 5f;

    private float displayWidth;
    private float displayHeight;
    private ILayout[] layouts = new ILayout[4];

    public interface ILayout
    {
        int GetMaxPlayers();
        Vector3[] GetPositions(int noOfPlayers);
        Quaternion[] GetRotations(int noOfPlayers);
        float GetMinHeight();
        float GetMinWidth();
        bool GetFreezePositionX(int noOfPlayers, int index);
    }

    private class Layout1 : ILayout
    {
        private Vector3[] playerPositions = new Vector3[4];
        private Quaternion[] rotations = new Quaternion[4];

        private float displayWidth;
        private float displayHeight;

        public Layout1(float displayWidth, float displayHeight)
        {
            this.displayWidth = displayWidth;
            this.displayHeight = displayHeight;
        }

        public bool GetFreezePositionX(int noOfPlayers, int index)
        {
            if (noOfPlayers < 3)
                return false;
            else
                return ((index == 0) || (index == 3));
        }

        public int GetMaxPlayers()
        {
            return 4;
        }

        public float GetMinHeight()
        {
            return playerHeight + playingAreaHeight;
        }

        public float GetMinWidth()
        {
            return 3 * padding + 2 * playerHeight + 2 * playerWidth;
        }

        public Vector3[] GetPositions(int noOfPlayers)
        {
            float xSpace = displayWidth - (2 * playerHeight + 2 * playerWidth);
            float xGap = xSpace / 5;
            float ySpace = displayHeight / 2 - playingAreaUpperLeftY - playerHeight;
            float yGap = ySpace / 2;

            float x0 = -1f * displayWidth / 2f + xGap + playerHeight / 2;
            float x3 = displayWidth / 2f - xGap - playerHeight / 2;

            float x1 = -1f * xGap / 2f - playerWidth / 2f;
            float y12 = displayHeight / 2f - yGap - playerHeight / 2f;

            if (noOfPlayers < 3)
            {
                playerPositions[0] = new Vector3(x1 - playerCorrection, y12, 0f);
                playerPositions[1] = new Vector3(-1f * x1 - playerCorrection, y12, 0f);
            }
            else
            {
                playerPositions[0] = new Vector3(x0, 0f, 0f);
                playerPositions[1] = new Vector3(x1 - playerCorrection, y12, 0f);
                playerPositions[2] = new Vector3(-1f * x1 - playerCorrection, y12, 0f);
                playerPositions[3] = new Vector3(x3, 3.8f, 0f);
            }

            return playerPositions;
        }

        public Quaternion[] GetRotations(int noOfPlayers)
        {
            if (noOfPlayers < 3)
            {
                rotations[0] = Quaternion.identity;
                rotations[1] = Quaternion.identity;
            }
            else
            {
                rotations[0] = Quaternion.Euler(new Vector3(0, 0, 90));
                rotations[1] = Quaternion.identity;
                rotations[2] = Quaternion.identity;
                rotations[3] = Quaternion.Euler(new Vector3(0, 0, 270));
            }

            return rotations;
        }
    }

    private class Layout2 : ILayout
    {
        private Vector3[] playerPositions = new Vector3[2];
        private Quaternion[] rotations = new Quaternion[2];

        private float displayWidth;
        private float displayHeight;

        public Layout2(float displayWidth, float displayHeight)
        {
            this.displayWidth = displayWidth;
            this.displayHeight = displayHeight;
        }

        public bool GetFreezePositionX(int noOfPlayers, int index)
        {
            return false;
        }

        public int GetMaxPlayers()
        {
            return 2;
        }

        public float GetMinHeight()
        {
            return playerHeight + playingAreaHeight;
        }

        public float GetMinWidth()
        {
            return padding + 2 * playerWidth;
        }

        public Vector3[] GetPositions(int noOfPlayers)
        {
            float xSpace = displayWidth - 2 * playerWidth;
            float xGap = xSpace / 3;
            float ySpace = displayHeight / 2 - playingAreaUpperLeftY - playerHeight;
            float yGap = ySpace / 2;

            float x1 = -1f * xGap / 2f - playerWidth / 2f;
            float y12 = displayHeight / 2f - yGap - playerHeight / 2f;

            playerPositions[0] = new Vector3(x1 - playerCorrection, y12, 0f);
            playerPositions[1] = new Vector3(-1f * x1 - playerCorrection, y12, 0f);

            return playerPositions;
        }

        public Quaternion[] GetRotations(int noOfPlayers)
        {
            rotations[0] = Quaternion.identity;
            rotations[1] = Quaternion.identity;
            return rotations;
        }
    }

    private class Layout3 : ILayout
    {
        private Vector3[] playerPositions = new Vector3[6];
        private Quaternion[] rotations = new Quaternion[6];

        private float displayWidth;
        private float displayHeight;

        public Layout3(float displayWidth, float displayHeight)
        {
            this.displayWidth = displayWidth;
            this.displayHeight = displayHeight;
        }

        public bool GetFreezePositionX(int noOfPlayers, int index)
        {
            return false;
        }

        public int GetMaxPlayers()
        {
            return 6;
        }

        public float GetMinHeight()
        {
            return 2 * playerHeight + padding;
        }

        public float GetMinWidth()
        {
            return 3 * padding + 4 * playerWidth;
        }

        public Vector3[] GetPositions(int noOfPlayers)
        {
            float xSpace = displayWidth - 4 * playerWidth;
            float xGap = xSpace / 5;

            float leftX = -1 * displayWidth / 2;
            float x0 = leftX + 1 * xGap + 0 * playerWidth + playerWidth / 2 - playerCorrection;
            float x1 = leftX + 2 * xGap + 1 * playerWidth + playerWidth / 2 - playerCorrection;
            float x2 = leftX + 3 * xGap + 2 * playerWidth + playerWidth / 2 - playerCorrection;
            float x3 = leftX + 4 * xGap + 3 * playerWidth + playerWidth / 2 - playerCorrection;

            float y0123 = displayHeight / 2f - padding - 0.5f * playerHeight;
            float y45 = displayHeight / 2f - 2 * padding - 1.5f * playerHeight;

            if (noOfPlayers == 2)
            {
                playerPositions[0] = new Vector3(x1, y0123, 0f);
                playerPositions[1] = new Vector3(x2, y0123, 0f);
            }
            else if (noOfPlayers <= 4)
            {
                playerPositions[0] = new Vector3(x0, y0123, 0f);
                playerPositions[1] = new Vector3(x1, y0123, 0f);
                playerPositions[2] = new Vector3(x2, y0123, 0f);
                playerPositions[3] = new Vector3(x3, y0123, 0f);

            }
            else
            {
                playerPositions[0] = new Vector3(x0, y45, 0f);
                playerPositions[1] = new Vector3(x0, y0123, 0f);
                playerPositions[2] = new Vector3(x1, y0123, 0f);
                playerPositions[3] = new Vector3(x2, y0123, 0f);
                playerPositions[4] = new Vector3(x3, y0123, 0f);
                playerPositions[5] = new Vector3(x3, y45, 0f);
            }

            return playerPositions;
        }

        public Quaternion[] GetRotations(int noOfPlayers)
        {
            rotations[0] = Quaternion.identity;
            rotations[1] = Quaternion.identity;
            rotations[2] = Quaternion.identity;
            rotations[3] = Quaternion.identity;
            rotations[4] = Quaternion.identity;
            rotations[5] = Quaternion.identity;

            return rotations;
        }
    }

    private class Layout4 : ILayout
    {
        private Vector3[] playerPositions = new Vector3[3];
        private Quaternion[] rotations = new Quaternion[3];

        private float displayWidth;
        private float displayHeight;

        public Layout4(float displayWidth, float displayHeight)
        {
            this.displayWidth = displayWidth;
            this.displayHeight = displayHeight;
        }

        public bool GetFreezePositionX(int noOfPlayers, int index)
        {
            return false;
        }

        public int GetMaxPlayers()
        {
            return 3;
        }

        public float GetMinHeight()
        {
            return 4 * playerHeight + playingAreaHeight + 3 * padding;
        }

        public float GetMinWidth()
        {
            return playingAreaWidth;
        }

        public Vector3[] GetPositions(int noOfPlayers)
        {
            float ySpace = displayHeight / 2 - playingAreaUpperLeftY - 2 * playerHeight;
            float yGap = ySpace / 3;

            float x = -1 * playerCorrection;
            float y0 = displayHeight / 2f - yGap - playerHeight / 2f;
            float y1 = displayHeight / 2f - 2 * yGap - 1.5f * playerHeight;
            float y2 = -1 * playingAreaHeight / 2 + padding;

            playerPositions[0] = new Vector3(x, y0, 0f);
            playerPositions[1] = new Vector3(x, y1, 0f);
            playerPositions[2] = new Vector3(x, y2, 0f);

            return playerPositions;
        }


        public Quaternion[] GetRotations(int noOfPlayers)
        {
            rotations[0] = Quaternion.identity;
            rotations[1] = Quaternion.identity;
            rotations[2] = Quaternion.identity;
            return rotations;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // get size of screen
        // player : 4,6 x 2,8

        RectTransform rt = gameObject.GetComponent<RectTransform>();
        var ls = rt.localScale;
        displayWidth = rt.rect.width * ls.x;
        displayHeight = rt.rect.height * ls.y;

        layouts[0] = new Layout3(displayWidth, displayHeight); // 6 players
        layouts[1] = new Layout1(displayWidth, displayHeight); // 4 players
        layouts[2] = new Layout4(displayWidth, displayHeight); // 3 players
        layouts[3] = new Layout2(displayWidth, displayHeight); // 2 players

        CoumputeLayout();
    }

    private void CoumputeLayout()
    {
        foreach (ILayout l in layouts)
            if ((l.GetMinWidth() <= displayWidth) && (l.GetMinHeight() <= displayHeight))
            {
                ONO.Current.SetLayout(l);
                return;
            }
        ONO.Current.SetLayout(null);
    }

}
