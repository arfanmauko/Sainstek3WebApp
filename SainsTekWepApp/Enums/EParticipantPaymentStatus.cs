using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SainsTekWepApp.Enums
{
    //Berisi status dari pendaftar sebagai peserta.
    public enum EParticipantPaymentStatus
    {
        //Baru aktif saat user mendaftar (default).
        //KonfirmasiPembayaran saat user telah melakukan konfirmasi pembayaran.
        //Ditolak saat KonfirmasiPembayaran user telah dicheck oleh admin dan ditolak.
        //Diterima saat KonfirmasiPembayaran user telah dicheck oleh admin dan diterima.

        Baru, KonfirmasiPembayaran, Ditolak, Diterima
    }
}