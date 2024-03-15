using System.Collections;
using UnityEngine;

namespace MultiplayerARPG.GameData.Model.Playables
{
    public partial class PlayableCharacterModel
    {
        public bool tidyPlayAnimationDirectlyRunning;
        [SerializeField] private bool tidyCurrentAnimationHasClip;

        public Coroutine PlayNpcActionAnimationDirectly(ActionAnimation actionAnimation, bool loop = false)
        {
            return StartActionCoroutine(PlayNpcActionAnimationRoutine(actionAnimation, loop));
        }

        private IEnumerator PlayNpcActionAnimationRoutine(ActionAnimation actionAnimation, bool loop = false)
        {
            float playSpeedMultiplier = (actionAnimation.GetAnimSpeedRate() > 0) ? actionAnimation.GetAnimSpeedRate() : 1f;
            //isDoingAction = true;
            AudioManager.PlaySfxClipAtAudioSource(actionAnimation.GetRandomAudioClip(), GenericAudioSource);

            tidyCurrentAnimationHasClip = actionAnimation.state.clip != null && animator.isActiveAndEnabled;

            if (tidyCurrentAnimationHasClip)
            {
                // Wait by animation playing duration
                yield return new WaitForSecondsRealtime(Behaviour.PlayAction(actionAnimation.state, playSpeedMultiplier, 0, loop));
                // Waits by current transition + extra duration before end playing animation state
                yield return new WaitForSecondsRealtime(actionAnimation.GetExtendDuration() / playSpeedMultiplier);
            }

            //isDoingAction = false;

            if(!loop) CancelPlayingNpcActionAnimationDirectly(true);
        }

        public void CancelPlayingNpcActionAnimationDirectly(bool stopActionAnimationIfPlaying)
        {
            if (tidyCurrentAnimationHasClip && stopActionAnimationIfPlaying)
            {
                Behaviour.StopAction();
                StopActionAnimation();
            }
            tidyPlayAnimationDirectlyRunning = false;
        }
    }
}