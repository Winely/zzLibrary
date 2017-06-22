// Encryptor.h : CEncryptor ������

#pragma once
#include "resource.h"       // ������



#include "Encryptor_i.h"



#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Windows CE ƽ̨(�粻�ṩ��ȫ DCOM ֧�ֵ� Windows Mobile ƽ̨)���޷���ȷ֧�ֵ��߳� COM ���󡣶��� _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA ��ǿ�� ATL ֧�ִ������߳� COM ����ʵ�ֲ�����ʹ���䵥�߳� COM ����ʵ�֡�rgs �ļ��е��߳�ģ���ѱ�����Ϊ��Free����ԭ���Ǹ�ģ���Ƿ� DCOM Windows CE ƽ̨֧�ֵ�Ψһ�߳�ģ�͡�"
#endif

using namespace ATL;


// CEncryptor

class ATL_NO_VTABLE CEncryptor :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CEncryptor, &CLSID_Encryptor>,
	public IDispatchImpl<IEncryptor, &IID_IEncryptor, &LIBID_EncryptorLib, /*wMajor =*/ 1, /*wMinor =*/ 0>
{
public:
	CEncryptor()
	{
	}

DECLARE_REGISTRY_RESOURCEID(IDR_ENCRYPTOR1)


BEGIN_COM_MAP(CEncryptor)
	COM_INTERFACE_ENTRY(IEncryptor)
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



	STDMETHOD(Add)(LONG a, LONG b, LONG* result);
};

OBJECT_ENTRY_AUTO(__uuidof(Encryptor), CEncryptor)
