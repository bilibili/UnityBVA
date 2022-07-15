using GLTF.Schema;
using GLTF.Extensions;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Newtonsoft.Json;

namespace BVA.Component
{
    public enum ContentType
    {
        Avatar,
        Scene,
        Game
    }
    public enum LegalUser
    {
        OnlyAuthor,
        LicensedPerson,
        Everyone,
    }
    public enum UsageScenario
    {
        LiveStream,
        Game,
        SocialNetwork
    }
    public enum LicenseType
    {
        Redistribution_Prohibited,
        CC0,
        CC_BY,
        CC_BY_NC,
        CC_BY_SA,
        CC_BY_NC_SA,
        CC_BY_ND,
        CC_BY_NC_ND,
        Other
    }

    public enum UsageLicense
    {
        disallow,
        allow,
    }
    [CreateAssetMenu(fileName = "MetaInfo", menuName = "BVA/MetaInfo")]
    public class BVAMetaInfoScriptableObject : ScriptableObject
    {
        public string formatVersion;
        public string title;
        public string version;
        public string author;
        public string contact;
        public string reference;
        public Texture2D thumbnail;
        public TextureId thumbnailId { private set; get; }
        public void SetTextureId(TextureId id)
        {
            thumbnailId = id;
        }
        public ContentType contentType;

        [Tooltip("What kind of person can use this asset")]
        public LegalUser legalUser;

        [Tooltip("Whether it is allowed in violent situations")]
        public UsageLicense violentUsage;

        [Tooltip("Whether it is allowed in sexual situations")]
        public UsageLicense sexualUsage;

        [Tooltip("Whether commercial use is permitted")]
        public UsageLicense commercialUsage;

        public LicenseType licenseType;
        public string customLicenseUrl;
        public BVAMetaInfoScriptableObject()
        {
            formatVersion = BVAConst.FORMAT_VERSION;
            title = "title";
            version = "1.0.0";
            author = "author";
            contact = "ai@bilibili.com";
            reference = "bilibili.com";
        }
        public JObject Serialize()
        {
            JObject jo = new JObject();
            jo.Add(nameof(formatVersion), formatVersion);
            jo.Add(nameof(title), title);
            jo.Add(nameof(version), version);
            jo.Add(nameof(author), author);
            jo.Add(nameof(contact), contact);
            jo.Add(nameof(reference), reference);
            if (thumbnail != null)
                jo.Add(nameof(thumbnail), thumbnailId.Id);
            jo.Add(nameof(contentType), contentType.ToString());
            jo.Add(nameof(legalUser), legalUser.ToString());
            jo.Add(nameof(violentUsage), violentUsage.ToString());
            jo.Add(nameof(sexualUsage), sexualUsage.ToString());
            jo.Add(nameof(commercialUsage), commercialUsage.ToString());
            jo.Add(nameof(licenseType), licenseType.ToString());
            jo.Add(nameof(customLicenseUrl), customLicenseUrl);
            return jo;
        }
    }

}