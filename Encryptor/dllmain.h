// dllmain.h: 模块类的声明。

class CEncryptorModule : public ATL::CAtlDllModuleT< CEncryptorModule >
{
public :
	DECLARE_LIBID(LIBID_EncryptorLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_ENCRYPTOR, "{47EB9FE8-47A3-45D8-B8E0-64FB137591B1}")
};

extern class CEncryptorModule _AtlModule;
