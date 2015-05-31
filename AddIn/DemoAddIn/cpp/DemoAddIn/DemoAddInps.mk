
DemoAddInps.dll: dlldata.obj DemoAddIn_p.obj DemoAddIn_i.obj
	link /dll /out:DemoAddInps.dll /def:DemoAddInps.def /entry:DllMain dlldata.obj DemoAddIn_p.obj DemoAddIn_i.obj \
		kernel32.lib rpcns4.lib rpcrt4.lib oleaut32.lib uuid.lib \
.c.obj:
	cl /c /Ox /DREGISTER_PROXY_DLL \
		$<

clean:
	@del DemoAddInps.dll
	@del DemoAddInps.lib
	@del DemoAddInps.exp
	@del dlldata.obj
	@del DemoAddIn_p.obj
	@del DemoAddIn_i.obj
