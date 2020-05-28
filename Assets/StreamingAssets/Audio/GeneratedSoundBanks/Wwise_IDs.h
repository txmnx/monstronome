/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID LEGATO = 1422035515U;
        static const AkUniqueID MUSIC_END_BEAT = 2368806795U;
        static const AkUniqueID MUSIC_INTRODUCTION_BEAT = 2768496808U;
        static const AkUniqueID MUSIC_TUNING = 2546653270U;
        static const AkUniqueID PAUSE_ALL = 3864097025U;
        static const AkUniqueID PIZZICATO = 2077337834U;
        static const AkUniqueID PLAY_METRONOME = 122105866U;
        static const AkUniqueID PLAY_MUSIC = 2932040671U;
        static const AkUniqueID RESUME_ALL = 3679762312U;
        static const AkUniqueID SETWOODS = 3271167387U;
        static const AkUniqueID SFX_BRASS_BROKEN = 2516234976U;
        static const AkUniqueID SFX_PERCUSSIONS_BROKEN = 212679835U;
        static const AkUniqueID SFX_POTION_BROKEN = 1231703344U;
        static const AkUniqueID SFX_POTION_COLLISION = 345071173U;
        static const AkUniqueID SFX_POTION_PICKUP = 3668990679U;
        static const AkUniqueID SFX_POTION_RIGHT = 3758674935U;
        static const AkUniqueID SFX_POTION_SHAKE = 3591561671U;
        static const AkUniqueID SFX_POTION_SPAWN = 2242916472U;
        static const AkUniqueID SFX_POTION_THROW = 3606142437U;
        static const AkUniqueID SFX_POTION_WRONG = 3427361090U;
        static const AkUniqueID SFX_STRINGS_BROKEN = 2404344115U;
        static const AkUniqueID SFX_WAND_ENTER = 3420964884U;
        static const AkUniqueID SFX_WAND_EXIT = 1368847222U;
        static const AkUniqueID SFX_WAND_IDDLE = 3638337738U;
        static const AkUniqueID SFX_WOODS_BROKEN = 2822419119U;
        static const AkUniqueID STACCATO = 56434811U;
        static const AkUniqueID STOPALL = 3086540886U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace FOCUS
        {
            static const AkUniqueID GROUP = 249970651U;

            namespace STATE
            {
                static const AkUniqueID FOCUS_BRASS = 2755940665U;
                static const AkUniqueID FOCUS_NONE = 3029345696U;
                static const AkUniqueID FOCUS_PERCUSSIONS = 3205378936U;
                static const AkUniqueID FOCUS_STRINGS = 2855506176U;
                static const AkUniqueID FOCUS_WOODS = 173669664U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace FOCUS

        namespace MUSIC
        {
            static const AkUniqueID GROUP = 3991942870U;

            namespace STATE
            {
                static const AkUniqueID METRONOME = 3537469747U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID START = 1281810935U;
            } // namespace STATE
        } // namespace MUSIC

        namespace POTIONCOUNT
        {
            static const AkUniqueID GROUP = 1090964253U;

            namespace STATE
            {
                static const AkUniqueID LEFT_0 = 50072827U;
                static const AkUniqueID LEFT_1 = 50072826U;
                static const AkUniqueID LEFT_2 = 50072825U;
                static const AkUniqueID LEFT_3 = 50072824U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace POTIONCOUNT

    } // namespace STATES

    namespace SWITCHES
    {
        namespace SW_ARTICULATION_BRASS
        {
            static const AkUniqueID GROUP = 554303915U;

            namespace SWITCH
            {
                static const AkUniqueID LEGATO = 1422035515U;
                static const AkUniqueID PIZZICATO = 2077337834U;
                static const AkUniqueID STACCATO = 56434811U;
            } // namespace SWITCH
        } // namespace SW_ARTICULATION_BRASS

        namespace SW_ARTICULATION_PERCUSSIONS
        {
            static const AkUniqueID GROUP = 2383379086U;

            namespace SWITCH
            {
                static const AkUniqueID LEGATO = 1422035515U;
                static const AkUniqueID PIZZICATO = 2077337834U;
                static const AkUniqueID STACCATO = 56434811U;
            } // namespace SWITCH
        } // namespace SW_ARTICULATION_PERCUSSIONS

        namespace SW_ARTICULATION_STRINGS
        {
            static const AkUniqueID GROUP = 1952059502U;

            namespace SWITCH
            {
                static const AkUniqueID LEGATO = 1422035515U;
                static const AkUniqueID PIZZICATO = 2077337834U;
                static const AkUniqueID STACCATO = 56434811U;
            } // namespace SWITCH
        } // namespace SW_ARTICULATION_STRINGS

        namespace SW_ARTICULATION_WOODS
        {
            static const AkUniqueID GROUP = 1043506402U;

            namespace SWITCH
            {
                static const AkUniqueID LEGATO = 1422035515U;
                static const AkUniqueID PIZZICATO = 2077337834U;
                static const AkUniqueID STACCATO = 56434811U;
            } // namespace SWITCH
        } // namespace SW_ARTICULATION_WOODS

        namespace SW_FAMILY_SOLIST
        {
            static const AkUniqueID GROUP = 2599669371U;

            namespace SWITCH
            {
                static const AkUniqueID BRASS = 505915784U;
                static const AkUniqueID NOBODY = 1182451328U;
                static const AkUniqueID PERCUSSIONS = 3762497105U;
                static const AkUniqueID STRINGS = 3363523641U;
                static const AkUniqueID WOODS = 2780586317U;
            } // namespace SWITCH
        } // namespace SW_FAMILY_SOLIST

        namespace SW_INTENSITY_BRASS
        {
            static const AkUniqueID GROUP = 4041549459U;

            namespace SWITCH
            {
                static const AkUniqueID FORTE = 297596479U;
                static const AkUniqueID MEZZOFORTE = 3831224266U;
                static const AkUniqueID PIANISSIMO = 2665367761U;
            } // namespace SWITCH
        } // namespace SW_INTENSITY_BRASS

        namespace SW_INTENSITY_PERCUSSIONS
        {
            static const AkUniqueID GROUP = 3232210438U;

            namespace SWITCH
            {
                static const AkUniqueID FORTE = 297596479U;
                static const AkUniqueID MEZZOFORTE = 3831224266U;
                static const AkUniqueID PIANISSIMO = 2665367761U;
            } // namespace SWITCH
        } // namespace SW_INTENSITY_PERCUSSIONS

        namespace SW_INTENSITY_STRINGS
        {
            static const AkUniqueID GROUP = 2452737974U;

            namespace SWITCH
            {
                static const AkUniqueID FORTE = 297596479U;
                static const AkUniqueID MEZZOFORTE = 3831224266U;
                static const AkUniqueID PIANISSIMO = 2665367761U;
            } // namespace SWITCH
        } // namespace SW_INTENSITY_STRINGS

        namespace SW_INTENSITY_WOODS
        {
            static const AkUniqueID GROUP = 3193984618U;

            namespace SWITCH
            {
                static const AkUniqueID FORTE = 297596479U;
                static const AkUniqueID MEZZOFORTE = 3831224266U;
                static const AkUniqueID PIANISSIMO = 2665367761U;
            } // namespace SWITCH
        } // namespace SW_INTENSITY_WOODS

        namespace SW_POTION_TYPE
        {
            static const AkUniqueID GROUP = 371597594U;

            namespace SWITCH
            {
                static const AkUniqueID ARTICULATION = 1762926010U;
                static const AkUniqueID BONUS = 2356001030U;
                static const AkUniqueID MALUS = 1680241497U;
            } // namespace SWITCH
        } // namespace SW_POTION_TYPE

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID RTPC_GETVOLUME_BRASS = 959410393U;
        static const AkUniqueID RTPC_GETVOLUME_PERCUSSIONS = 2898731032U;
        static const AkUniqueID RTPC_GETVOLUME_STRINGS = 3691157920U;
        static const AkUniqueID RTPC_GETVOLUME_WOODS = 2247649216U;
        static const AkUniqueID RTPC_INTENSITY_BRASS = 3154931128U;
        static const AkUniqueID RTPC_INTENSITY_PERCUSSIONS = 2676178657U;
        static const AkUniqueID RTPC_INTENSITY_STRINGS = 234013449U;
        static const AkUniqueID RTPC_INTENSITY_WOODS = 1164038749U;
        static const AkUniqueID RTPC_POTION_SPEED = 2031370766U;
        static const AkUniqueID RTPC_TEMPO = 3945680200U;
        static const AkUniqueID SIDECHAIN_SFX_MUSIC = 3190805563U;
        static const AkUniqueID SIDECHAIN_THEME_BASEMUSIC = 2701297324U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID SNB_MUSIC_BASEORCHESTRAL = 3881725531U;
        static const AkUniqueID SNB_MUSIC_SECONDARYORCHESTRAL = 3806678136U;
        static const AkUniqueID SNB_SFX = 3710691832U;
        static const AkUniqueID SNB_STRUCTURE = 1080929334U;
        static const AkUniqueID SNB_UI = 2467341413U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID AMBISONICS_BUS = 3539010900U;
        static const AkUniqueID BRASS_ORCHESTRAL = 1346697092U;
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID METRONOME = 3537469747U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID ORCHESTRAL = 2542362038U;
        static const AkUniqueID PERCUSSIONS_ORCHESTRAL = 4206581327U;
        static const AkUniqueID SFX = 393239870U;
        static const AkUniqueID STRINGS_ORCHESTRAL = 1192927207U;
        static const AkUniqueID THEME = 1319017392U;
        static const AkUniqueID UI = 1551306167U;
        static const AkUniqueID WOODS_ORCHESTRAL = 1033152923U;
    } // namespace BUSSES

    namespace AUX_BUSSES
    {
        static const AkUniqueID REVERB_CONCERTHALL = 44205819U;
    } // namespace AUX_BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
