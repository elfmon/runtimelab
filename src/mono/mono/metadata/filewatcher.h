/*
 * filewatcher.h: File System Watcher internal calls
 *
 * Authors:
 *	Gonzalo Paniagua Javier (gonzalo@ximian.com)
 *
 * (C) 2004 Novell, Inc. (http://www.novell.com)
 */

#ifndef _MONO_METADATA_FILEWATCHER_H
#define _MONO_METADATA_FILEWATCHER_H

#include <mono/metadata/object.h>
#include <mono/io-layer/io-layer.h>

#ifdef HAVE_UNISTD_H
#include <unistd.h>
#endif

#ifdef HAVE_SYS_SYSCALL_H
#include <sys/syscall.h>
#endif

G_BEGIN_DECLS

gboolean ves_icall_System_IO_FSW_SupportsFSW (void);

gpointer ves_icall_System_IO_FSW_OpenDirectory (MonoString *path, gpointer reserved);

gboolean ves_icall_System_IO_FSW_CloseDirectory (gpointer handle);

gboolean ves_icall_System_IO_FSW_ReadDirectoryChanges (
						gpointer handle,
						MonoArray *buffer,
						gboolean includeSubdirs,
						gint filters,
						gpointer overlap,
						gpointer callback);

gboolean ves_icall_System_IO_FAMW_InternalFAMNextEvent (gpointer conn,
							MonoString **filename,
							gint *code,
							gint *reqnum);

int ves_icall_System_IO_InotifyWatcher_GetInotifyInstance (void);
int ves_icall_System_IO_InotifyWatcher_AddWatch (int fd, MonoString *directory, gint32 mask);
int ves_icall_System_IO_InotifyWatcher_RemoveWatch (int fd, gint32 watch_descriptor);

G_END_DECLS

#endif

