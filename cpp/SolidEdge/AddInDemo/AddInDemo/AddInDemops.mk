
AddInDemops.dll: dlldata.obj AddInDemo_p.obj AddInDemo_i.obj
	link /dll /out:AddInDemops.dll /def:AddInDemops.def /entry:DllMain dlldata.obj AddInDemo_p.obj AddInDemo_i.obj \
		kernel32.lib rpcns4.lib rpcrt4.lib oleaut32.lib uuid.lib \
.c.obj:
	cl /c /Ox /DREGISTER_PROXY_DLL \
		$<

clean:
	@del AddInDemops.dll
	@del AddInDemops.lib
	@del AddInDemops.exp
	@del dlldata.obj
	@del AddInDemo_p.obj
	@del AddInDemo_i.obj
