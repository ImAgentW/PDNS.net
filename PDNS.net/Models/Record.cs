using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PDNS.net.Models
{
    [Table("Records")]
    public class Record
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Domain")]
        public int Domain_ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public int TTL { get; set; }

        [DefaultValue(0)]
        public int Prio { get; set; }
        public byte Disabled { get; set; }
        public string OrderName { get; set; }

        [DefaultValue(1)]
        public byte Auth { get; set; }
        public int Change_Date { get; set; }

        public Domain Domain { get; set; }

        public enum RecordType
        {
            A,
            AAAA,
            AFSDB,
            ALIAS,
            APL,
            CAA,
            CERT,
            CDNSKEY,
            CDS,
            CNAME,
            DNSKEY,
            DNAME,
            DS,
            HINFO,
            KEY,
            LOC,
            MX,
            NAPTR,
            NS,
            NSEC_NSEC_NSECPARAM,
            OPENPGPKEY,
            PTR,
            RP,
            RRSIG,
            SOA,
            SPF,
            SSHFP,
            SRV,
            TKEY_TSIG,
            TLSA,
            SMIMEA,
            TXT,
            URI,
            A6,
            DHCID,
            DLV,
            EUI48_EUI64,
            IPSECKEY,
            KX,
            MAILA,
            MAILB,
            MINFO,
            MR,
            RKEY,
            SIG,
            WKS
        }
    }
}
