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
        static const AkUniqueID PLAY_MUSIC = 2932040671U;
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

        namespace GAME_STATE
        {
            static const AkUniqueID GROUP = 766723505U;

            namespace STATE
            {
                static const AkUniqueID END = 529726532U;
                static const AkUniqueID MIDDLE = 1026627430U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID START = 1281810935U;
                static const AkUniqueID TENSE = 391998042U;
            } // namespace STATE
        } // namespace GAME_STATE

    } // namespace STATES

    namespace SWITCHES
    {
        namespace SW_ARTICULATION_BRASS
        {
            static const AkUniqueID GROUP = 554303915U;

            namespace SWITCH
            {
                static const AkUniqueID FLUTTERMUTE = 4028101794U;
                static const AkUniqueID HARMONMUTE = 3644683267U;
                static const AkUniqueID LEGATO = 1422035515U;
                static const AkUniqueID STACCATORIPS = 3432601699U;
            } // namespace SWITCH
        } // namespace SW_ARTICULATION_BRASS

        namespace SW_ARTICULATION_STRINGS
        {
            static const AkUniqueID GROUP = 1952059502U;

            namespace SWITCH
            {
                static const AkUniqueID LEGATO = 1422035515U;
                static const AkUniqueID PIZZICATO = 2077337834U;
                static const AkUniqueID STACCATO = 56434811U;
                static const AkUniqueID TREMOLO = 4056223263U;
            } // namespace SWITCH
        } // namespace SW_ARTICULATION_STRINGS

        namespace SW_ARTICULATION_WOODS
        {
            static const AkUniqueID GROUP = 1043506402U;

            namespace SWITCH
            {
                static const AkUniqueID FLUTTER = 3466625877U;
                static const AkUniqueID LEGATO = 1422035515U;
                static const AkUniqueID STACCATO = 56434811U;
                static const AkUniqueID TRILL = 3997891192U;
            } // namespace SWITCH
        } // namespace SW_ARTICULATION_WOODS

        namespace SW_INTENSITY_BRASS
        {
            static const AkUniqueID GROUP = 4041549459U;

            namespace SWITCH
            {
                static const AkUniqueID HIGHT = 2915956327U;
                static const AkUniqueID HUGE = 3009393504U;
                static const AkUniqueID LOW = 545371365U;
                static const AkUniqueID MIDDLE = 1026627430U;
            } // namespace SWITCH
        } // namespace SW_INTENSITY_BRASS

        namespace SW_INTENSITY_PERCUSSIONS
        {
            static const AkUniqueID GROUP = 3232210438U;

            namespace SWITCH
            {
                static const AkUniqueID HIGHT = 2915956327U;
                static const AkUniqueID HUGE = 3009393504U;
                static const AkUniqueID LOW = 545371365U;
                static const AkUniqueID MIDDLE = 1026627430U;
            } // namespace SWITCH
        } // namespace SW_INTENSITY_PERCUSSIONS

        namespace SW_INTENSITY_STRINGS
        {
            static const AkUniqueID GROUP = 2452737974U;

            namespace SWITCH
            {
                static const AkUniqueID HIGHT = 2915956327U;
                static const AkUniqueID HUGE = 3009393504U;
                static const AkUniqueID LOW = 545371365U;
                static const AkUniqueID MIDDLE = 1026627430U;
            } // namespace SWITCH
        } // namespace SW_INTENSITY_STRINGS

        namespace SW_INTENSITY_WOODS
        {
            static const AkUniqueID GROUP = 3193984618U;

            namespace SWITCH
            {
                static const AkUniqueID HIGHT = 2915956327U;
                static const AkUniqueID HUGE = 3009393504U;
                static const AkUniqueID LOW = 545371365U;
                static const AkUniqueID MIDDLE = 1026627430U;
            } // namespace SWITCH
        } // namespace SW_INTENSITY_WOODS

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID RTPC_ARTICULATION_BRASS = 2436025794U;
        static const AkUniqueID RTPC_ARTICULATION_STRINGS = 2116129127U;
        static const AkUniqueID RTPC_ARTICULATION_WOODS = 1437958447U;
        static const AkUniqueID RTPC_GETVOLUME_BRASS = 959410393U;
        static const AkUniqueID RTPC_GETVOLUME_PERCUSSIONS = 2898731032U;
        static const AkUniqueID RTPC_GETVOLUME_STRINGS = 3691157920U;
        static const AkUniqueID RTPC_GETVOLUME_WOODS = 2247649216U;
        static const AkUniqueID RTPC_INTENSITY_BRASS = 3154931128U;
        static const AkUniqueID RTPC_INTENSITY_PERCUSSIONS = 2676178657U;
        static const AkUniqueID RTPC_INTENSITY_STRINGS = 234013449U;
        static const AkUniqueID RTPC_INTENSITY_WOODS = 1164038749U;
        static const AkUniqueID RTPC_TEMPO = 3945680200U;
        static const AkUniqueID RTPC_TIME_DELAY_BRASS = 666250906U;
        static const AkUniqueID RTPC_TIME_DELAY_PERCUSSIONS = 4041181711U;
        static const AkUniqueID RTPC_TIME_DELAY_STRINGS = 3714523071U;
        static const AkUniqueID RTPC_TIME_DELAY_WOODS = 3650061639U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID MONSTRONOME_SNB = 3550178992U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID AMBISONICS_BUS = 3539010900U;
        static const AkUniqueID BRASS = 505915784U;
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID PERCUSSIONS = 3762497105U;
        static const AkUniqueID STRINGS = 3363523641U;
        static const AkUniqueID WOOD = 2058049674U;
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
