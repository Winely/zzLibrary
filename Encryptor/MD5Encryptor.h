// MD5Encryptor.h : CMD5Encryptor ������

#pragma once
#include "resource.h"       // ������


#include <string>
#include "Encryptor_i.h"



#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Windows CE ƽ̨(�粻�ṩ��ȫ DCOM ֧�ֵ� Windows Mobile ƽ̨)���޷���ȷ֧�ֵ��߳� COM ���󡣶��� _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA ��ǿ�� ATL ֧�ִ������߳� COM ����ʵ�ֲ�����ʹ���䵥�߳� COM ����ʵ�֡�rgs �ļ��е��߳�ģ���ѱ�����Ϊ��Free����ԭ���Ǹ�ģ���Ƿ� DCOM Windows CE ƽ̨֧�ֵ�Ψһ�߳�ģ�͡�"
#endif

using namespace ATL;
using namespace std;


// CMD5Encryptor

class ATL_NO_VTABLE CMD5Encryptor :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CMD5Encryptor, &CLSID_MD5Encryptor>,
	public IDispatchImpl<IMD5Encryptor, &IID_IMD5Encryptor, &LIBID_EncryptorLib, /*wMajor =*/ 1, /*wMinor =*/ 0>
{
public:
	CMD5Encryptor();

DECLARE_REGISTRY_RESOURCEID(IDR_MD5ENCRYPTOR)


BEGIN_COM_MAP(CMD5Encryptor)
	COM_INTERFACE_ENTRY(IMD5Encryptor)
	COM_INTERFACE_ENTRY(IDispatch)
END_COM_MAP()



	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}

	void FinalRelease()
	{
	}

public:
	//! construct a MD5 from any buffer
	void GenerateMD5(unsigned char* buffer, int bufferlen);
	
	//! construct a md5src from char *
	CMD5Encryptor(const char * md5src);

	//! construct a CMD5Encryptor from a 16 bytes md5
	CMD5Encryptor(unsigned long* md5src);

	//! give the value from equer
	// void operator =(CMD5Encryptor equer);

	//! to a string
	string ToString();
	
	CMD5Encryptor* CreateInstance();

	unsigned long m_data[4];

private:
#define uint8  unsigned char
#define uint32 unsigned long int

	struct md5_context
	{
		uint32 total[2];
		uint32 state[4];
		uint8 buffer[64];
	};

	void md5_starts(struct md5_context *ctx);
	void md5_process(struct md5_context *ctx, uint8 data[64]);
	void md5_update(struct md5_context *ctx, uint8 *input, uint32 length);
	void md5_finish(struct md5_context *ctx, uint8 digest[16]);

public:
	STDMETHOD(encrypt)(BSTR data, BSTR* result);
};



OBJECT_ENTRY_AUTO(__uuidof(MD5Encryptor), CMD5Encryptor)
